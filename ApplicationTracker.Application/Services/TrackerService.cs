using ApplicationTracker.Application.DTO;
using ApplicationTracker.Application.Interfaces;
using ApplicationTracker.Application.Requests;
using ApplicationTracker.Application.ViewModels;
using ApplicationTracker.Data.Interfaces;
using ApplicationTracker.Data.Requests.Applications;
using ApplicationTracker.Data.Requests.ApplicationEvents;
using ApplicationTracker.Data.Requests.Stages;
using ApplicationTracker.Data.Rows;
using ApplicationTracker.Domain.Constants;


namespace ApplicationTracker.Application.Services;

public class TrackerService : ITrackerService
{
    private readonly IDataAccess _dataAccess;

    public TrackerService(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
    }

    // --------------------------------------------------
    // Applications
    // --------------------------------------------------
    public async Task<List<ApplicationDto>> GetAllApplicationsAsync()
    {
        var request = new ReturnAllApplicationsRequest();
        var rows = await _dataAccess.FetchListAsync<Application_Row>(request);

        return rows
            .Select(r => new ApplicationDto
            {
                ApplicationId = r.ApplicationId,
                CompanyName = r.CompanyName,
                JobTitle = r.JobTitle
            })
            .ToList();
    }

    public async Task<int> InsertApplicationAsync(CreateApplicationRequest requestModel)
    {
        if (requestModel is null)
            throw new ArgumentNullException(nameof(requestModel));

        // 1. Insert application
        var newAppId = await InsertApplicationCoreAsync(requestModel);

        // 2. Load stages and build the path of stage keys
        var stagesByKey = await GetStagesByKeyAsync();
        var targetStage = FindTargetStage(stagesByKey, requestModel.StageId);
        var pathKeys = BuildPathKeys(targetStage.StageKey);

        // 3. Insert events in the chosen path order
        await InsertApplicationEventsAsync(newAppId, pathKeys, stagesByKey);

        return newAppId;
    }


    private async Task<int> InsertApplicationCoreAsync(CreateApplicationRequest requestModel)
    {
        var appRow = new Application_Row
        {
            CompanyName = requestModel.CompanyName,
            JobTitle = requestModel.JobTitle
        };

        var insertAppRequest = new InsertApplicationRequest(appRow);
        // Using FetchAsync here so the INSERT returns the new ApplicationId
        var newAppId = await _dataAccess.FetchAsync<int>(insertAppRequest);

        if (newAppId == 0)
            throw new InvalidOperationException("Failed to insert application.");

        return newAppId;
    }

    private async Task<Dictionary<string, Stage_Row>> GetStagesByKeyAsync()
    {
        var stagesRequest = new ReturnAllStagesRequest();
        var stageRows = await _dataAccess.FetchListAsync<Stage_Row>(stagesRequest);

        // Converts the list into a dictionary:
        // key   = StageKey
        // value = full Stage_Row
        // lookup is case-insensitive because of StringComparer.OrdinalIgnoreCase
        return stageRows.ToDictionary(
            s => s.StageKey,
            StringComparer.OrdinalIgnoreCase);
    }

    private static Stage_Row FindTargetStage(IDictionary<string, Stage_Row> stagesByKey, int targetStageId)
    {
        var targetStage = stagesByKey
            .Values
            .FirstOrDefault(s => s.StageId == targetStageId);

        if (targetStage is null)
            throw new InvalidOperationException($"StageId {targetStageId} not found.");

        return targetStage;
    }

    private static readonly string[] PipelineKeys =
    {
        StageKeys.Applied,
        StageKeys.PhoneScreen,
        StageKeys.TechnicalInterview,
        StageKeys.OnSite,
        StageKeys.Offer
    };

    private static List<string> BuildPathKeys(string targetStageKey)
    {
        var pathKeys = new List<string>();

        // If final is one of the pipeline stages, stop at that point
        if (PipelineKeys.Contains(targetStageKey, StringComparer.OrdinalIgnoreCase))
        {
            // e.g. final = Technical Interview → Applied -> Phone -> TechnicalInterview
            foreach (var key in PipelineKeys)
            {
                pathKeys.Add(key);
                if (key.Equals(targetStageKey, StringComparison.OrdinalIgnoreCase))
                    break;
            }
        }
        else if (targetStageKey == StageKeys.NoResponse)
        {
            // Applied -> No response
            pathKeys.Add(StageKeys.Applied);
            pathKeys.Add(StageKeys.NoResponse);
        }
        else if (targetStageKey == StageKeys.Accepted ||
                 targetStageKey == StageKeys.RejectedOffer)
        {
            // Full pipeline to Offer, then the terminal stage (Accepted or RejectedOffer)
            foreach (var key in PipelineKeys)
            {
                pathKeys.Add(key);
            }
            pathKeys.Add(targetStageKey);
        }
        else
        {
            throw new InvalidOperationException($"Unsupported stage key {targetStageKey}");
        }

        return pathKeys;
    }

    private async Task InsertApplicationEventsAsync(int applicationId, IEnumerable<string> pathKeys, IReadOnlyDictionary<string, Stage_Row> stagesByKey)
    {
        foreach (var key in pathKeys)
        {
            if (!stagesByKey.TryGetValue(key, out var stage))
                throw new InvalidOperationException($"Stage '{key}' not found in Stages table.");

            var eventRow = new ApplicationEvent_Row
            {
                ApplicationId = applicationId,
                StageId = stage.StageId
            };

            var insertEventRequest = new InsertApplicationEventRequest(eventRow);
            await _dataAccess.ExecuteAsync(insertEventRequest);
        }
    }

  

    // --------------------------------------------------
    // Stages
    // --------------------------------------------------
    public async Task<List<StageDto>> GetAllStagesAsync()
    {
        var request = new ReturnAllStagesRequest();
        var rows = await _dataAccess.FetchListAsync<Stage_Row>(request);

        return rows
            .Select(r => new StageDto
            {
                StageId = r.StageId,
                StageKey = r.StageKey,
                DisplayName = r.DisplayName,
                SortOrder = r.SortOrder
            })
            .OrderBy(s => s.SortOrder)
            .ToList();
    }

    // --------------------------------------------------
    // Events
    // --------------------------------------------------
    public async Task AddApplicationEventAsync(AddApplicationEventRequest requestModel)
    {
        if (requestModel is null) throw new ArgumentNullException(nameof(requestModel));

        var row = new ApplicationEvent_Row
        {
            ApplicationId = requestModel.ApplicationId,
            StageId = requestModel.StageId
        };

        var request = new InsertApplicationEventRequest(row);
        await _dataAccess.ExecuteAsync(request);
    }

    // --------------------------------------------------
    // Analytics: Timelines
    // --------------------------------------------------
    public async Task<List<ApplicationTimelineDto>> GetApplicationTimelinesAsync()
    {
        var request = new ReturnApplicationTimelinesRequest();
        var rows = await _dataAccess.FetchListAsync<ApplicationTimeline_Row>(request);

        var lookup = new Dictionary<int, ApplicationTimelineDto>();

        foreach (var row in rows)
        {
            if (!lookup.TryGetValue(row.ApplicationId, out var app))
            {
                app = new ApplicationTimelineDto
                {
                    ApplicationId = row.ApplicationId,
                    CompanyName = row.CompanyName,
                    JobTitle = row.JobTitle
                };
                lookup.Add(row.ApplicationId, app);
            }

            app.Events.Add(new StageEventDto
            {
                EventId = row.EventId,
                StageId = row.StageId,
                StageKey = row.StageKey,
                DisplayName = row.DisplayName,
                SortOrder = row.SortOrder
            });
        }

        foreach (var app in lookup.Values)
        {
            app.Events = app.Events
                .OrderBy(e => e.SortOrder)
                .ThenBy(e => e.EventId)
                .ToList();
        }

        return lookup.Values
            .OrderBy(a => a.ApplicationId)
            .ToList();
    }

    // --------------------------------------------------
    // Analytics: Funnel (highest stage per app)
    // --------------------------------------------------
    public async Task<List<StageStatsDto>> GetStageFunnelAsync()
    {
        var request = new ReturnStageFunnelStatsRequest();
        var rows = await _dataAccess.FetchListAsync<StageFunnelStat_Row>(request);

        return rows
            .Select(r => new StageStatsDto
            {
                StageKey = r.StageKey,
                DisplayName = r.DisplayName,
                ApplicationCount = r.ApplicationCount
            })
            .ToList();
    }

    // --------------------------------------------------
    // Analytics: Sankey links
    // --------------------------------------------------
    public async Task<List<SankeyLinkViewModel>> GetSankeyLinksAsync()
    {
        var request = new ReturnApplicationTimelinesRequest();
        var rows = (await _dataAccess.FetchListAsync<ApplicationTimeline_Row>(request))
            .ToList();

        if (!rows.Any())
            return new List<SankeyLinkViewModel>();

        // Group all events per application
        var byApp = rows.GroupBy(r => r.ApplicationId);

        var transitions = new Dictionary<(string From, string To), int>();

        foreach (var appEvents in byApp)
        {
            var ordered = appEvents
                .OrderBy(e => e.SortOrder)
                .ThenBy(e => e.EventId)
                .ToList();

            // Need at least 2 events to form 1 edge
            for (int i = 0; i < ordered.Count - 1; i++)
            {
                var from = ordered[i].DisplayName;
                var to = ordered[i + 1].DisplayName;

                var key = (from, to);
                transitions.TryGetValue(key, out var count);
                transitions[key] = count + 1;
            }
        }

        return transitions
            .Select(t => new SankeyLinkViewModel
            {
                From = t.Key.From,
                To = t.Key.To,
                Count = t.Value
            })
            .OrderBy(l => l.From)
            .ThenBy(l => l.To)
            .ToList();
    }

}
