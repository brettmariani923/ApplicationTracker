using ApplicationTracker.Data.Interfaces;
using ApplicationTracker.Data.Rows;

namespace ApplicationTracker.Data.Requests.ReturnAllApplicationTimelinesRequest;

public class ReturnApplicationTimelinesRequest : IDataFetchList<ApplicationTimeline_Row>
{
    public string GetSql() => @"
        SELECT 
            a.ApplicationId,
            a.CompanyName,
            a.JobTitle,
            e.EventId,
            e.StageId,
            s.StageKey,
            s.DisplayName,
            s.SortOrder
        FROM dbo.Applications a
        JOIN dbo.ApplicationEvents e ON a.ApplicationId = e.ApplicationId
        JOIN dbo.Stages s           ON e.StageId       = s.StageId
        ORDER BY a.ApplicationId, s.SortOrder, e.EventId;";

    public object? GetParameters() => null;
}
