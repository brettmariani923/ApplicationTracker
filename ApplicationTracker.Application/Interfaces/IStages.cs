using ApplicationTracker.Application.DTO;

namespace ApplicationTracker.Application.Interfaces
{
    public interface IStages
    {
        Task<List<StageDto>> GetAllStagesAsync();
    }
}
