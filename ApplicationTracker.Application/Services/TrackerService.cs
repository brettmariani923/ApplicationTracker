using ApplicationTracker.Data.Requests;
using ApplicationTracker.Data.Rows;
using ApplicationTracker.Data.Interfaces;
using ApplicationTracker.Data.DTO;

namespace ApplicationTracker.Application.Services
{
    public class TrackerService
    {
        private readonly IDataAccess _dataAccess;

        public async Task<List<Application_DTO>> GetAllApplicationsAsync()
        {
            var request = new ReturnAllApplicationsRequest();
            var result = await _dataAccess.FetchListAsync<Application_DTO>(request);
            return result.ToList();
        }

        public async Task InsertApplicationAsync(Application_Row model)
        {
            var request = new InsertApplicationRequest(model);
            var result = await _dataAccess.ExecuteAsync(request);
        }
    }
}
