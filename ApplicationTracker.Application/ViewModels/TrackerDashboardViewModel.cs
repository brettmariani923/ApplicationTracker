using ApplicationTracker.Application.DTO;

namespace ApplicationTracker.Application.ViewModels;

public class TrackerDashboardViewModel
{
    public List<ApplicationTimelineDto> Timelines { get; set; } = new();
    public List<StageStatsDto> FunnelStats { get; set; } = new();
    public List<SankeyLinkViewModel> SankeyLinks { get; set; } = new();

    public List<StageDto> Stages { get; set; } = new();

    public CreateApplicationRequest NewApplication { get; set; } = new();
}
