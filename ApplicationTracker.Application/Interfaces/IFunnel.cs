using ApplicationTracker.Application.DTO;

namespace ApplicationTracker.Application.Interfaces
{
    public interface IFunnel
    {
        Task<List<StageStatsDto>> GetStageFunnelAsync();
    }
}
