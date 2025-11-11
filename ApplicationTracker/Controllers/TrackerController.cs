using Microsoft.AspNetCore.Mvc;
using ApplicationTracker.Data.Interfaces;

namespace ApplicationTracker.Controllers
{
    public class TrackerController : Controller
    {
        private readonly IDataAccess _data;
        private readonly ITrackerService _trackerService;
        public TrackerController(IDataAccess data, ItrackerService trackerService)
        {
            _data = data;
            _trackerService = trackerService;
        }

        public async Task<IActionResult> Index()
        {
            var jobs = await _trackerService.GetAllApplicationsAsync();
            return View(jobs);
        }



    }
}
