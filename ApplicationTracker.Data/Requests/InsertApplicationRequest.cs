using ApplicationTracker.Data.Interfaces;
using ApplicationTracker.Data.Rows;

namespace ApplicationTracker.Data.Requests.Applications;

public class InsertApplicationRequest : IDataFetch<int>
{
    private readonly Application_Row _row;

    public InsertApplicationRequest(Application_Row row)
    {
        _row = row ?? throw new ArgumentNullException(nameof(row));
    }

    public string GetSql() => @"
        INSERT INTO dbo.Applications (CompanyName, JobTitle)
        OUTPUT INSERTED.ApplicationId
        VALUES (@CompanyName, @JobTitle);";

    public object? GetParameters() => new
    {
        _row.CompanyName,
        _row.JobTitle
    };
}
