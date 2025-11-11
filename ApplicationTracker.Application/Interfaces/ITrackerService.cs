using ApplicationTracker.Data.DTO;

namespace ApplicationTracker.Application.Interfaces
{
    public interface ITrackerService
    {
        Task<List<Application_DTO>> GetAllApplicationsAsync();
    }
}
