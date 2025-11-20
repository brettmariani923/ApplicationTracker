using ApplicationTracker.Data.Rows;
using ApplicationTracker.Data.Interfaces;

public class UpdateApplicationRequest : IDataExecute
{
    private readonly Application_Row _row;

    public UpdateApplicationRequest(Application_Row row)
    {
        _row = row;
    }

    public string GetSql() =>
        @"UPDATE dbo.Applications
          SET CompanyName = @CompanyName,
              JobTitle = @JobTitle
          WHERE ApplicationId = @ApplicationId;";

    public object? GetParameters() => new
    {
        _row.ApplicationId,
        _row.CompanyName,
        _row.JobTitle
    };
}
