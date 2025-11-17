using ApplicationTracker.Application.DTO;
using ApplicationTracker.Application.Interfaces;
using ApplicationTracker.Application.Requests;
using ApplicationTracker.Application.ViewModels;
using ApplicationTracker.Data.Interfaces;
using ApplicationTracker.Data.Requests.Applications;
using ApplicationTracker.Data.Requests.ApplicationEvents;
using ApplicationTracker.Data.Requests.Stages;
using ApplicationTracker.Data.Rows;

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

    public async Task InsertApplicationAsync(CreateApplicationRequest requestModel)
    {
        if (requestModel is null) throw new ArgumentNullException(nameof(requestModel));

        var row = new Application_Row
        {
            CompanyName = requestModel.CompanyName,
            JobTitle = requestModel.JobTitle
        };

        var request = new InsertApplicationRequest(row);
        await _dataAccess.ExecuteAsync(request);
        // Optionally return new ID in the future.
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
        var rows = await _dataAccess.FetchListAsync<ApplicationTimeline_Row>(request);

        var byApp = rows
            .GroupBy(r => r.ApplicationId)
            .ToList();

        var transitions = new Dictionary<(string From, string To), int>();

        foreach (var appEvents in byApp)
        {
            var ordered = appEvents
                .OrderBy(e => e.SortOrder)
                .ThenBy(e => e.EventId)
                .ToList();

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
