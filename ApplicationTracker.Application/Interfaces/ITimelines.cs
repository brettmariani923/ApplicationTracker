using ApplicationTracker.Application.DTO;

namespace ApplicationTracker.Application.Interfaces
{
    public interface ITimelines
    {
        Task<List<ApplicationTimelineDto>> GetApplicationTimelinesAsync();
    }
}
