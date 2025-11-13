namespace ApplicationTracker.Application.DTO;

public class StageEventDto
{
    public long EventId { get; set; }
    public int StageId { get; set; }
    public string StageKey { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public int SortOrder { get; set; }
}
