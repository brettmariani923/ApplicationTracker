using ApplicationTracker.Data.Interfaces;
using ApplicationTracker.Data.Rows;

namespace ApplicationTracker.Data.Requests.Stages;

public class ReturnAllStagesRequest : IDataFetchList<Stage_Row>
{
    public string GetSql() => @"
        SELECT StageId, StageKey, DisplayName, SortOrder
        FROM dbo.Stages
        ORDER BY SortOrder;";

    public object? GetParameters() => null;
}
