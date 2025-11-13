using ApplicationTracker.Application.DTO;
using ApplicationTracker.Application.Requests;
using ApplicationTracker.Application.ViewModels;

namespace ApplicationTracker.Application.Interfaces;

public interface ITrackerService
{
    // Basic application CRUD-ish operations
    Task<List<ApplicationDto>> GetAllApplicationsAsync();
    Task InsertApplicationAsync(CreateApplicationRequest request);

    // Stages
    Task<List<StageDto>> GetAllStagesAsync();

    // Events
    Task AddApplicationEventAsync(AddApplicationEventRequest request);

    // Analytics / views
    Task<List<ApplicationTimelineDto>> GetApplicationTimelinesAsync();
    Task<List<StageStatsDto>> GetStageFunnelAsync();
    Task<List<SankeyLinkViewModel>> GetSankeyLinksAsync();
}
