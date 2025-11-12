using ApplicationTracker.Data.Requests;
using ApplicationTracker.Data.DTO;
using ApplicationTracker.Data.Interfaces;

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

        public async Task InsertApplicationAsync()
        {
            var request = new InsertApplicationRequest();
            var result = await _dataAccess.ExecuteAsync
        }
    }
}
