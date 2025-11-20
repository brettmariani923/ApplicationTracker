using ApplicationTracker.Application.DTO;
using ApplicationTracker.Data.Rows;

namespace ApplicationTracker.Application.Interfaces
{
    public interface IApplications
    {
        Task<List<ApplicationDto>> GetAllApplicationsAsync();
        Task<int> InsertApplicationAsync(CreateApplicationRequest requestModel);
        Task DeleteApplicationAsync(int applicationId);
        Task UpdateApplicationAsync(Application_Row row);
        Task<Application_Row?> GetApplicationByIdAsync(int id);

    }
}
