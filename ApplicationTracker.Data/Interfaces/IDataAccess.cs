namespace ApplicationTracker.Data.Interfaces
{
    internal interface IDataAccess
    {
        public Task<int> ExecuteAsync(IDataExecute request);
        public Task<TResponse?> FetchAsync<TResponse>(IDataFetch<TResponse> request);
        public Task<IEnumerable<TResponse>> FetchListAsync<TResponse>(IDataFetchList<TResponse> request);
    }
}
