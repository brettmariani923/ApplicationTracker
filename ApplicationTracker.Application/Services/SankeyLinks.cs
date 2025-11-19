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


namespace ApplicationTracker.Application.Services
{
    public class SankeyLinks
    {
        private readonly IDataAccess _dataAccess;

        public SankeyLinks(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
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
}
}
