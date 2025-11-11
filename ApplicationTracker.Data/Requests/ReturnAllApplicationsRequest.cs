using ApplicationTracker.Data.Interfaces;
using ApplicationTracker.Data.DTO;

namespace ApplicationTracker.Data.Requests
{
    public class ReturnAllApplicationsRequest : IDataFetchList<Application_DTO>
    {
        public string GetSql()
        {
            return "Select * FROM dbo.Applications";
        }

        public object? GetParameters()
        {
            return null;
        }
    }
}
