using ApplicationTracker.Application.DTO;

namespace ApplicationTracker.Application.Interfaces
{
    public interface IApplications
    {
        Task<List<ApplicationDto>> GetAllApplicationsAsync();
        Task<int> InsertApplicationAsync(CreateApplicationRequest requestModel);
    }
}
