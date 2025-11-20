using ApplicationTracker.Application.ViewModels;

namespace ApplicationTracker.Application.Interfaces
{
    public interface ISankeyLinks
    {
        Task<List<SankeyLinkViewModel>> GetSankeyLinksAsync();
    }
}
