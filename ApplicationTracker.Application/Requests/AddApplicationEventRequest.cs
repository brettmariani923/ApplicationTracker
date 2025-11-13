namespace ApplicationTracker.Application.Requests;

public class AddApplicationEventRequest
{
    public int ApplicationId { get; set; }
    public int StageId { get; set; }
}
