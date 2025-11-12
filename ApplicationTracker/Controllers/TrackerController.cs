using Microsoft.AspNetCore.Mvc;
using ApplicationTracker.Data.Interfaces;
using ApplicationTracker.Application.Interfaces;
using ApplicationTracker.Data.Rows;

namespace ApplicationTracker.Controllers
{
    public class TrackerController : Controller
    {
        private readonly IDataAccess _data;
        private readonly ITrackerService _trackerService;

        public TrackerController(IDataAccess data, ITrackerService trackerService)
        {
            _data = data;
            _trackerService = trackerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var jobs = await _trackerService.GetAllApplicationsAsync();
            return View(jobs);
        }

        [HttpPost]
        public async Task<IActionResult> AddApplication( )
        {
            await _trackerService.InsertApplicationAsync();
            return View();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateApplication()
        {
            await _trackerService.UpdateApplicationAsync();
            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteApplication()
        {
            await _trackerService.DeleteApplicationAsync();
            return View();
        }


    }
}
