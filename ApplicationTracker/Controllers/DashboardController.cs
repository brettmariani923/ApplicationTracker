using ApplicationTracker.Application.Interfaces;
using ApplicationTracker.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationTracker.Api.Controllers;

public class DashboardController : Controller
{
    private readonly IDashboardService _dashboard;

    public DashboardController(IDashboardService dashboard)
    {
        _dashboard = dashboard;
    }

    public async Task<IActionResult> Index()
        => View(await _dashboard.GetDashboardAsync());

    public async Task<IActionResult> Timeline()
        => View(await _dashboard.GetTimelineAsync());

    [HttpPost]
    public async Task<IActionResult> CreateApplication(DashboardViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Index", await _dashboard.GetDashboardAsync());

        await _dashboard.CreateApplicationAsync(model.NewApplication);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> UpdateApplication(int id)
        => View("Timeline", await _dashboard.GetUpdateAsync(id));
    [HttpPost]
    public async Task<IActionResult> UpdateApplicationPost(DashboardViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Timeline", await _dashboard.GetUpdateAsync(model.UpdateApplication.ApplicationId));

        await _dashboard.UpdateApplicationAsync(model.UpdateApplication);
        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    public async Task<IActionResult> DeleteApplication(int applicationId)
    {
        await _dashboard.DeleteApplicationAsync(applicationId);
        return RedirectToAction(nameof(Index));
    }
}

