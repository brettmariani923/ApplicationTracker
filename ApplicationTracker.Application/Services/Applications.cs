using ApplicationTracker.Application.DTO;
using ApplicationTracker.Application.Interfaces;
using ApplicationTracker.Data.Interfaces;
using ApplicationTracker.Data.Requests.ReturnAllApplicationsRequest;
using ApplicationTracker.Data.Requests.DeleteApplicationRequest;
using ApplicationTracker.Data.Requests.InsertApplicationRequest;
using ApplicationTracker.Data.Requests.InsertApplicationEventRequest;
using ApplicationTracker.Data.Requests.ReturnAllStagesRequest;
using ApplicationTracker.Data.Requests;
using ApplicationTracker.Data.Rows;
using ApplicationTracker.Domain.Constants;

namespace ApplicationTracker.Application.Services
{
    public class Applications : IApplications
    {
        private readonly IDataAccess _dataAccess;

        public Applications(IDataAccess dataAccess)
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

        //This is telling us which of our stagekey values form the main ordered pipeline for our graph
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

        public async Task DeleteApplicationAsync(int applicationId)
        {
            var request = new DeleteApplicationRequest(applicationId);
            await _dataAccess.ExecuteAsync(request);
        }

        public async Task UpdateApplicationAsync(Application_Row updated)
        {
            if (updated is null)
                throw new ArgumentNullException(nameof(updated));

            // 1. Load existing application data (including last stage)
            var existing = await GetApplicationByIdAsync(updated.ApplicationId);
            if (existing == null)
                throw new InvalidOperationException($"Application {updated.ApplicationId} not found.");

            // 2. Update Applications table (CompanyName / JobTitle only)
            var updateRequest = new UpdateApplicationRequest(updated);
            await _dataAccess.ExecuteAsync(updateRequest);

            // 3. If the stage changed, add a new ApplicationEvent
            if (updated.StageId != 0 && updated.StageId != existing.StageId)
            {
                var eventRow = new ApplicationEvent_Row
                {
                    ApplicationId = updated.ApplicationId,
                    StageId = updated.StageId
                };

                var insertEventRequest = new InsertApplicationEventRequest(eventRow);
                await _dataAccess.ExecuteAsync(insertEventRequest);
            }
        }

        public async Task<Application_Row?> GetApplicationByIdAsync(int id)
        {
            var request = new ReturnApplicationByIdRequest(id);
            var result = await _dataAccess.FetchAsync<Application_Row>(request);
            return result;
        }

    }
}
