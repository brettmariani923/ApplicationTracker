using ApplicationTracker.Data.Interfaces;
using ApplicationTracker.Data.Rows;

namespace ApplicationTracker.Data.Requests.ReturnStageFunnelStatsRequest;

public class ReturnStageFunnelStatsRequest : IDataFetchList<StageFunnelStat_Row>
{
    public string GetSql() => @"
        WITH LatestStagePerApp AS (
            SELECT
                a.ApplicationId,
                MAX(s.SortOrder) AS MaxSortOrder
            FROM dbo.Applications a
            JOIN dbo.ApplicationEvents e ON a.ApplicationId = e.ApplicationId
            JOIN dbo.Stages           s ON e.StageId = s.StageId
            GROUP BY a.ApplicationId
        )
        SELECT
            s.StageKey,
            s.DisplayName,
            COUNT(*) AS ApplicationCount
        FROM LatestStagePerApp l
        JOIN dbo.Stages s ON l.MaxSortOrder = s.SortOrder
        GROUP BY s.StageKey, s.DisplayName, s.SortOrder
        ORDER BY s.SortOrder;";

    public object? GetParameters() => null;
}
