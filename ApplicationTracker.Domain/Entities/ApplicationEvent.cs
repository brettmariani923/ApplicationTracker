namespace ApplicationTracker.Domain.Entities;

public class ApplicationEvent
{
    public long EventId { get; set; }
    public int ApplicationId { get; set; }
    public int StageId { get; set; }
}
