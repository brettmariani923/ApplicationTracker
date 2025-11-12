using ApplicationTracker.Data.Rows;
using ApplicationTracker.Data.Interfaces;

namespace ApplicationTracker.Data.Requests
{
    public class InsertApplicationRequest : IDataExecute
    {
        private readonly Application_Row _row;

        public InsertApplicationRequest(Application_Row row)
        {
            _row = row;
        }
        public string GetSql() => @"INSERT INTO dbo.Applications (CompanyName, JobTitle)" + "Values(@CompanyName, @JobTitle);";
        
        public object? GetParameters() => new
        {
            _row.CompanyName,
            _row.JobTitle
        };

    }
}
