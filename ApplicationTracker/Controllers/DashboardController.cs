using ApplicationTracker.Application.Interfaces;
using ApplicationTracker.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ApplicationTracker.Data.Rows;

namespace ApplicationTracker.Api.Controllers;

public class DashboardController : Controller
{
    private readonly IApplications _applications;
    private readonly IFunnel _funnel;
    private readonly ISankeyLinks _sankey;
    private readonly IStages _stages;
    private readonly ITimelines _timelines;

    public DashboardController(IApplications applications, IFunnel funnel, ISankeyLinks sankey, IStages stages, ITimelines timelines)
    {
        _applications = applications ?? throw new ArgumentNullException(nameof(applications));
        _funnel = funnel ?? throw new ArgumentNullException(nameof(funnel));
        _sankey = sankey ?? throw new ArgumentNullException(nameof(sankey));
        _stages = stages ?? throw new ArgumentNullException(nameof(stages));
        _timelines = timelines ?? throw new ArgumentNullException(nameof(timelines));

    }

    // GET: /Dashboard
    public async Task<IActionResult> Index()
    {
        var timelines = await _timelines.GetApplicationTimelinesAsync();
        var funnel = await _funnel.GetStageFunnelAsync();
        var sankey = await _sankey.GetSankeyLinksAsync();
        var stages = await _stages.GetAllStagesAsync();

        var vm = new DashboardViewModel
        {
            Timelines = timelines,
            FunnelStats = funnel,
            SankeyLinks = sankey,
            Stages = stages,
            NewApplication = new CreateApplicationRequest()
        };

        return View(vm);
    }

    // Get: /Timeline
    public async Task<IActionResult> Timeline()
    {
        var timelines = await _timelines.GetApplicationTimelinesAsync();
        var stages = await _stages.GetAllStagesAsync();

        var vm = new DashboardViewModel
        {
            Timelines = timelines,
            Stages = stages
        };

        return View(vm);
    }


    // POST: /Dashboard/CreateApplication
    [HttpPost]
    public async Task<IActionResult> CreateApplication(DashboardViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // reload analytics + stages so the view can render again
            model.Timelines = await _timelines.GetApplicationTimelinesAsync();
            model.FunnelStats = await _funnel.GetStageFunnelAsync();
            model.SankeyLinks = await _sankey.GetSankeyLinksAsync();
            model.Stages = await _stages.GetAllStagesAsync();

            return View("Index", model);
        }

        // iinsert Application + ApplicationEvent
        await _applications.InsertApplicationAsync(model.NewApplication);

        // PRG pattern to avoid repost on refresh and to reload charts
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteApplication(int applicationId)
    {
        await _applications.DeleteApplicationAsync(applicationId);

        return RedirectToAction(nameof(Index)); // PRG pattern
    }

    [HttpGet]
    public async Task<IActionResult> UpdateApplication(int id)
    {
        var app = await _applications.GetApplicationByIdAsync(id);

        if (app == null)
            return NotFound();

        var vm = new DashboardViewModel
        {
            Timelines = await _timelines.GetApplicationTimelinesAsync(),
            FunnelStats = await _funnel.GetStageFunnelAsync(),
            SankeyLinks = await _sankey.GetSankeyLinksAsync(),
            Stages = await _stages.GetAllStagesAsync(),

            NewApplication = new CreateApplicationRequest(),
            UpdateApplication = app
        };

        return View("Timeline", vm);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateApplicationPost(DashboardViewModel model)
    {
        var updated = model.UpdateApplication;

        if (!ModelState.IsValid)
        {
            var vm = new DashboardViewModel
            {
                Timelines = await _timelines.GetApplicationTimelinesAsync(),
                FunnelStats = await _funnel.GetStageFunnelAsync(),
                SankeyLinks = await _sankey.GetSankeyLinksAsync(),
                Stages = await _stages.GetAllStagesAsync(),

                NewApplication = new CreateApplicationRequest(),
                UpdateApplication = updated
            };

            return View("Timeline", vm);
        }

        await _applications.UpdateApplicationAsync(updated);

        return RedirectToAction(nameof(Index));
    }





}

