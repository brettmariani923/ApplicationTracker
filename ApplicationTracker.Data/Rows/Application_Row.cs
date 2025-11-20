namespace ApplicationTracker.Data.Rows;

public class Application_Row
{
    public int ApplicationId { get; set; }
    public string CompanyName { get; set; } = "";
    public string? JobTitle { get; set; }
    public int StageId { get; set; }
}
