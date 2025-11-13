using ApplicationTracker.Application.Interfaces;
using ApplicationTracker.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationTracker.Api.Controllers;

// NOTE: this is NOT an [ApiController]. It's an MVC controller for Razor views.
public class DashboardController : Controller
{
    private readonly ITrackerService _tracker;

    public DashboardController(ITrackerService tracker)
    {
        _tracker = tracker ?? throw new ArgumentNullException(nameof(tracker));
    }

    // GET: /Dashboard
    public async Task<IActionResult> Index()
    {
        var timelines = await _tracker.GetApplicationTimelinesAsync();
        var funnel = await _tracker.GetStageFunnelAsync();
        var sankey = await _tracker.GetSankeyLinksAsync(); // or whatever you named it

        var vm = new TrackerDashboardViewModel
        {
            Timelines = timelines,
            FunnelStats = funnel,
            SankeyLinks = sankey
        };

        return View(vm);
    }

}
