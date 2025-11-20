using ApplicationTracker.Data.Interfaces;
using ApplicationTracker.Data.Rows;

namespace ApplicationTracker.Data.Requests
{
    public class ReturnApplicationByIdRequest : IDataFetch<Application_Row>
    {
        private readonly int _applicationId;

        public ReturnApplicationByIdRequest(int applicationId)
        {
            _applicationId = applicationId;
        }

        public string GetSql() =>
            @"SELECT a.ApplicationId,
                   a.CompanyName,
                   a.JobTitle,
                   (
                       SELECT TOP 1 StageId 
                       FROM ApplicationEvents 
                       WHERE ApplicationId = a.ApplicationId
                       ORDER BY EventId DESC
                   ) AS StageId
                FROM Applications a
                WHERE a.ApplicationId = @ApplicationId;
                ";

        public object? GetParameters() => new
        {
            ApplicationId = _applicationId
        };
    }
}
