using ApplicationTracker.Application.Interfaces;
using ApplicationTracker.Application.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationEventsController : ControllerBase
{
    private readonly ITrackerService _tracker;

    public ApplicationEventsController(ITrackerService tracker)
    {
        _tracker = tracker ?? throw new ArgumentNullException(nameof(tracker));
    }

    // POST: api/application-events
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] AddApplicationEventRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await _tracker.AddApplicationEventAsync(request);
        return NoContent();
    }
}
