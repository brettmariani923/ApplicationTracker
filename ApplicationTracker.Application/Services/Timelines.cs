using ApplicationTracker.Application.DTO;
using ApplicationTracker.Application.Interfaces;
using ApplicationTracker.Data.Requests.ReturnAllApplicationTimelinesRequest;
using ApplicationTracker.Data.Interfaces;
using ApplicationTracker.Data.Rows;

namespace ApplicationTracker.Application.Services
{
    public class Timelines : ITimelines
    {
        private readonly IDataAccess _dataAccess;

        public Timelines(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
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
    }
}
