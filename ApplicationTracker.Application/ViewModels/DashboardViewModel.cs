using ApplicationTracker.Application.DTO;
using ApplicationTracker.Data.Rows;

namespace ApplicationTracker.Application.ViewModels;

public class DashboardViewModel
{
    public List<ApplicationTimelineDto> Timelines { get; set; } = new();
    public List<StageStatsDto> FunnelStats { get; set; } = new();
    public List<SankeyLinkViewModel> SankeyLinks { get; set; } = new();

    public List<StageDto> Stages { get; set; } = new();

    public CreateApplicationRequest NewApplication { get; set; } = new();

    public Application_Row? UpdateApplication { get; set; }

}
