using ApplicationTracker.Data.Rows;
using ApplicationTracker.Data.DTO;

namespace ApplicationTracker.Application.Interfaces
{
    public interface ITrackerService
    {
        Task<List<Application_DTO>> GetAllApplicationsAsync();
        Task<List<Application_Row>> InsertApplicationAsync();
        Task<List<Application_Row>> UpdateApplicationAsync();
        Task<List<Application_Row>> DeleteApplicationAsync();

    }
}
