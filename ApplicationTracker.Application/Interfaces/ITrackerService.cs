using ApplicationTracker.Data.Rows;

namespace ApplicationTracker.Application.Interfaces
{
    public interface ITrackerService
    {
        Task<List<Application_Row>> GetAllApplicationsAsync();
    }
}
