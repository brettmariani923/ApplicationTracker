namespace ApplicationTracker.Domain.Entities;

public class Stage
{
    public int StageId { get; set; }
    public string StageKey { get; set; } = "";   
    public string DisplayName { get; set; } = "";   
    public int SortOrder { get; set; }              
}
