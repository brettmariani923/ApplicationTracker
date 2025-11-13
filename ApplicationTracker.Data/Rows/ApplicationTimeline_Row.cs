namespace ApplicationTracker.Data.Rows;

public class ApplicationTimeline_Row
{
    public int ApplicationId { get; set; }
    public string CompanyName { get; set; } = "";
    public string? JobTitle { get; set; }

    public long EventId { get; set; }
    public int StageId { get; set; }
    public string StageKey { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public int SortOrder { get; set; }
}
