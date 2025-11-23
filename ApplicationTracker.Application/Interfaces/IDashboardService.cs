using ApplicationTracker.Application.ViewModels;
using ApplicationTracker.Data.Rows;

namespace ApplicationTracker.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardAsync();
        Task<DashboardViewModel> GetTimelineAsync();
        Task CreateApplicationAsync(CreateApplicationRequest request);
        Task<DashboardViewModel> GetUpdateAsync(int appId);
        Task UpdateApplicationAsync(Application_Row update);
        Task DeleteApplicationAsync(int id);
    }
}
