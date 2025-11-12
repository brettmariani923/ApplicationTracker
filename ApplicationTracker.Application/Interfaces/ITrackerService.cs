using ApplicationTracker.Data.Rows;
using ApplicationTracker.Data.DTO;

namespace ApplicationTracker.Application.Interfaces
{
    public interface ITrackerService
    {
        Task<List<Application_Row>> GetAllApplicationsAsync();
        Task<List<Application_DTO>> InsertApplicationAsync()

    }
}
