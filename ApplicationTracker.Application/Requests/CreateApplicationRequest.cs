namespace ApplicationTracker.Application.Requests;

public class CreateApplicationRequest
{
    public string CompanyName { get; set; } = "";
    public string? JobTitle { get; set; }
}
