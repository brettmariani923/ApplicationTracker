using ApplicationTracker.Application.DTO;
using ApplicationTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrackerController : ControllerBase
{
    private readonly ITrackerService _tracker;

    public TrackerController(ITrackerService tracker)
    {
        _tracker = tracker ?? throw new ArgumentNullException(nameof(tracker));
    }

    // GET: api/tracker/timelines
    // Returns each application + ordered list of stages it reached
    [HttpGet("timelines")]
    public async Task<ActionResult<IEnumerable<ApplicationTimelineDto>>> GetTimelines()
    {
        var timelines = await _tracker.GetApplicationTimelinesAsync();
        return Ok(timelines);
    }

    // GET: api/tracker/stats/funnel
    // Returns per-stage counts based on highest stage each app reached
    [HttpGet("stats/funnel")]
    public async Task<ActionResult<IEnumerable<StageStatsDto>>> GetStageFunnel()
    {
        var stats = await _tracker.GetStageFunnelAsync();
        return Ok(stats);
    }
}
