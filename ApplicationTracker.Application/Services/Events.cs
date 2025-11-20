using ApplicationTracker.Application.Interfaces;
using ApplicationTracker.Application.Requests;
using ApplicationTracker.Data.Interfaces;
using ApplicationTracker.Data.Requests.InsertApplicationEventRequest;
using ApplicationTracker.Data.Rows;

namespace ApplicationTracker.Application.Services
{
    public class Events : IEvents
    {
        private readonly IDataAccess _dataAccess;

        public Events(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
        }

        // --------------------------------------------------
        // Events
        // --------------------------------------------------
        public async Task AddApplicationEventAsync(AddApplicationEventRequest requestModel)
        {
            if (requestModel is null) throw new ArgumentNullException(nameof(requestModel));

            var row = new ApplicationEvent_Row
            {
                ApplicationId = requestModel.ApplicationId,
                StageId = requestModel.StageId
            };

            var request = new InsertApplicationEventRequest(row);
            await _dataAccess.ExecuteAsync(request);
        }
    }
}
