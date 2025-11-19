using ApplicationTracker.Application.Interfaces;
using ApplicationTracker.Application.Requests;
using ApplicationTracker.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationTracker.Api.Controllers;

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
        var sankey = await _tracker.GetSankeyLinksAsync();
        var stages = await _tracker.GetAllStagesAsync();

        var vm = new TrackerDashboardViewModel
        {
            Timelines = timelines,
            FunnelStats = funnel,
            SankeyLinks = sankey,
            Stages = stages,
            NewApplication = new CreateApplicationRequest()
        };

        return View(vm);
    }


    // POST: /Dashboard/CreateApplication
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateApplication(TrackerDashboardViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // reload analytics + stages so the view can render again
            model.Timelines = await _tracker.GetApplicationTimelinesAsync();
            model.FunnelStats = await _tracker.GetStageFunnelAsync();
            model.SankeyLinks = await _tracker.GetSankeyLinksAsync();
            model.Stages = await _tracker.GetAllStagesAsync();

            return View("Index", model);
        }

        // iinsert Application + ApplicationEvent
        await _tracker.InsertApplicationAsync(model.NewApplication);

        // PRG pattern to avoid repost on refresh and to reload charts
        return RedirectToAction(nameof(Index));
    }

}

