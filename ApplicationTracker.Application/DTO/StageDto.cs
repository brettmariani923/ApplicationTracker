namespace ApplicationTracker.Application.DTO;

public class StageDto
{
    public int StageId { get; set; }
    public string StageKey { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public int SortOrder { get; set; }
}
