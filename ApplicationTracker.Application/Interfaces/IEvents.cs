using ApplicationTracker.Application.Requests;

namespace ApplicationTracker.Application.Interfaces
{
    public interface IEvents
    {
        Task AddApplicationEventAsync(AddApplicationEventRequest request);
    }
}
