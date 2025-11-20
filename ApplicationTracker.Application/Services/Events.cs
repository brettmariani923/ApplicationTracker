using ApplicationTracker.Application.DTO;
using ApplicationTracker.Application.Interfaces;
using ApplicationTracker.Application.Requests;
using ApplicationTracker.Application.ViewModels;
using ApplicationTracker.Data.Interfaces;
using ApplicationTracker.Data.Requests.Applications;
using ApplicationTracker.Data.Requests.ApplicationEvents;
using ApplicationTracker.Data.Requests.Stages;
using ApplicationTracker.Data.Rows;
using ApplicationTracker.Domain.Constants;



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
