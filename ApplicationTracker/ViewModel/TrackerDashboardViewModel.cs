using ApplicationTracker.Application.DTO;

namespace ApplicationTracker.Api.ViewModels;

public class TrackerDashboardViewModel
{
    public List<ApplicationTimelineDto> Timelines { get; set; } = new();
    public List<StageStatsDto> FunnelStats { get; set; } = new();
}
