using ApplicationTracker.Data.Interfaces;
using ApplicationTracker.Data.Rows;

namespace ApplicationTracker.Data.Requests.ApplicationEvents;

public class InsertApplicationEventRequest : IDataExecute
{
    private readonly ApplicationEvent_Row _row;

    public InsertApplicationEventRequest(ApplicationEvent_Row row)
    {
        _row = row ?? throw new ArgumentNullException(nameof(row));
    }

    public string GetSql() => @"
        INSERT INTO dbo.ApplicationEvents (ApplicationId, StageId)
        VALUES (@ApplicationId, @StageId);";

    public object? GetParameters() => new
    {
        _row.ApplicationId,
        _row.StageId
    };
}
