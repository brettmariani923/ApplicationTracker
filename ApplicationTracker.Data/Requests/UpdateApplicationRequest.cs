using ApplicationTracker.Data.Interfaces;
using ApplicationTracker.Data.Rows;

namespace ApplicationTracker.Data.Requests
{
    public class UpdateApplicationRequest
    {
        private readonly Application_Row _row;
        public UpdateApplicationRequest(Application_Row row)
        {
            _row = row;
        }

        public string GetSql() =>
        
            @"UPDATE dbo.Applications
              SET CompanyName = @CompanyName,
                     JobTitle = @JobTitle,
              WHERE CompanyName = @CompanyName;";
       
        public object? GetParameters() => new
        {
            _row.CompanyName,
            _row.JobTitle
        };
    }
}
