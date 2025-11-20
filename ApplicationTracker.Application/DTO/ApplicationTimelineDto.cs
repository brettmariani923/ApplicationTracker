namespace ApplicationTracker.Application.DTO;

public class ApplicationTimelineDto
{
    public int ApplicationId { get; set; }
    public string CompanyName { get; set; } = "";
    public string? JobTitle { get; set; }

    public List<StageEventDto> Events { get; set; } = new();

}
