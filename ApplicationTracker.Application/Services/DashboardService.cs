using ApplicationTracker.Application.ViewModels;
using ApplicationTracker.Data.Rows;
using ApplicationTracker.Application.Interfaces;


namespace ApplicationTracker.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IApplications _applications;
        private readonly IFunnel _funnel;
        private readonly ISankeyLinks _sankey;
        private readonly IStages _stages;
        private readonly ITimelines _timelines;

        public DashboardService(
            IApplications applications,
            IFunnel funnel,
            ISankeyLinks sankey,
            IStages stages,
            ITimelines timelines)
        {
            _applications = applications;
            _funnel = funnel;
            _sankey = sankey;
            _stages = stages;
            _timelines = timelines;
        }

        public async Task<DashboardViewModel> GetDashboardAsync()
        {
            return new DashboardViewModel
            {
                Timelines = await _timelines.GetApplicationTimelinesAsync(),
                FunnelStats = await _funnel.GetStageFunnelAsync(),
                SankeyLinks = await _sankey.GetSankeyLinksAsync(),
                Stages = await _stages.GetAllStagesAsync(),
                NewApplication = new CreateApplicationRequest()
            };
        }

        public async Task<DashboardViewModel> GetTimelineAsync()
        {
            return new DashboardViewModel
            {
                Timelines = await _timelines.GetApplicationTimelinesAsync(),
                Stages = await _stages.GetAllStagesAsync(),
            };
        }

        public Task CreateApplicationAsync(CreateApplicationRequest request)
            => _applications.InsertApplicationAsync(request);

        public async Task<DashboardViewModel> GetUpdateAsync(int appId)
        {
            var app = await _applications.GetApplicationByIdAsync(appId);

            return new DashboardViewModel
            {
                Timelines = await _timelines.GetApplicationTimelinesAsync(),
                FunnelStats = await _funnel.GetStageFunnelAsync(),
                SankeyLinks = await _sankey.GetSankeyLinksAsync(),
                Stages = await _stages.GetAllStagesAsync(),
                UpdateApplication = app
            };
        }

        public Task UpdateApplicationAsync(Application_Row update)
            => _applications.UpdateApplicationAsync(update);

        public Task DeleteApplicationAsync(int id)
            => _applications.DeleteApplicationAsync(id);
    }

}
