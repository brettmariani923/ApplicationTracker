using ApplicationTracker.Data.DTO;
using ApplicationTracker.Data.Interfaces;

namespace ApplicationTracker.Data.Requests
{
    public class DeleteApplicationRequest : IDataExecute
    {
        private readonly Application_DTO _dto;

        public DeleteApplicationRequest(Application_DTO dto)
        {
            _dto = dto;
        }

        public string GetSql()
        {
            return @"DELETE FROM dbo.Applications
                     WHERE ApplicationID = @ApplicationId;";
        }

        public object? GetParameters()
        {
            return new
            {
                _dto.ApplicationId
            };
        }
    }
}
