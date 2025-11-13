using ApplicationTracker.Data.Interfaces;
using ApplicationTracker.Data.Rows;

namespace ApplicationTracker.Data.Requests.Applications;

public class ReturnAllApplicationsRequest : IDataFetchList<Application_Row>
{
    public string GetSql() => @"
        SELECT ApplicationId, CompanyName, JobTitle
        FROM dbo.Applications
        ORDER BY ApplicationId;";

    public object? GetParameters() => null;
}

