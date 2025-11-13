namespace ApplicationTracker.Data.Rows;

public class Stage_Row
{
    public int StageId { get; set; }
    public string StageKey { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public int SortOrder { get; set; }
}
