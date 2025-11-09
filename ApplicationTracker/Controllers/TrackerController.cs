using Microsoft.AspNetCore.Mvc;
using ApplicationTracker.Data.Interfaces;

namespace ApplicationTracker.Controllers
{
    public class TrackerController : Controller
    {
        private readonly IDataAccess _data;
        public TrackerController(IDataAccess data)
        {
            _data = data;
        }



    }
}
