namespace ApplicationTracker.Domain.Entities;

public class Application
{
    public int ApplicationId { get; set; }
    public string CompanyName { get; set; } = "";
    public string? JobTitle { get; set; }
}
