using ApplicationTracker.Data.Interfaces;

namespace ApplicationTracker.Data.Requests;

public class DeleteApplicationRequest : IDataExecute
{
    private readonly int _applicationId;

    public DeleteApplicationRequest(int applicationId)
    {
        _applicationId = applicationId;
    }

    public string GetSql()
    {
        return @"DELETE FROM dbo.Applications
                 WHERE ApplicationId = @ApplicationId;";
    }

    public object? GetParameters()
    {
        return new
        {
            ApplicationId = _applicationId
        };
    }
}
