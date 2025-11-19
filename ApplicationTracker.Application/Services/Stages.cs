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
    public class Stages
    {
        private readonly IDataAccess _dataAccess;

        public Stages(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
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

    }
}
