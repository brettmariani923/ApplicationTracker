using ApplicationTracker.Application.DTO;
using ApplicationTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StagesController : ControllerBase
{
    private readonly ITrackerService _tracker;

    public StagesController(ITrackerService tracker)
    {
        _tracker = tracker ?? throw new ArgumentNullException(nameof(tracker));
    }

    // GET: api/stages
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StageDto>>> GetAll()
    {
        var stages = await _tracker.GetAllStagesAsync();
        return Ok(stages);
    }
}
