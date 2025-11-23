using ApplicationTracker.Application.DTO;
using ApplicationTracker.Application.Interfaces;
using ApplicationTracker.Data.Interfaces;
using ApplicationTracker.Data.Requests.ReturnStageFunnelStatsRequest;
using ApplicationTracker.Data.Rows;

namespace ApplicationTracker.Application.Services
{
    public class Funnel : IFunnel
    {
        private readonly IDataAccess _dataAccess;

        public Funnel(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
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
    }
}
