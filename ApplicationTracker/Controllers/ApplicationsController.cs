using ApplicationTracker.Application.DTO;
using ApplicationTracker.Application.Interfaces;
using ApplicationTracker.Application.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly ITrackerService _tracker;

    public ApplicationsController(ITrackerService tracker)
    {
        _tracker = tracker ?? throw new ArgumentNullException(nameof(tracker));
    }

    // GET: api/applications
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApplicationDto>>> GetAll()
    {
        var apps = await _tracker.GetAllApplicationsAsync();
        return Ok(apps);
    }

    // POST: api/applications
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateApplicationRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await _tracker.InsertApplicationAsync(request);

        return NoContent();
    }
}
