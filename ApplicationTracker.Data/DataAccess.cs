using Dapper;

namespace ApplicationTracker.Data
{
    public class DataAccess
    {
        private readonly IDbConnectionFactory _connectionFactory;
        DataAccess(IDbConnectionFactory connectionFactory) => _connectionFactory = connectionFactory;
        async task<int> ExecuteAsync(IDataExecute request) => await HandleRequest(async _ => await _.ExecuteAsync(request.GetSql(), request.GetParameters()));
        async task<TResponse> FetchAsync<TResponse>(IDataFetch<TResponse> request) => await HandleRequest(async _ => await _.QueryFirstOrDefaultAsync<TResponse>(request.GetSql(), request.GetParameters()));
        async task<IEnumerable<TResponse>> FetchListAsync<TResponse>(IDataFetchList<TResponse> request) => await HandleRequest(async _ => await _.QueryAsync<TResponse>(request.GetSql(), request.GetParameters()));
    
        private async Task<T> HandleRequest<T>(Func<IDbConnection, Task<T>> funcHandleRequest)
        {
            using var connection = _connectionFactory.NewConnection();

            connection.Open();

            return await funcHandleRequest.Invoke(connection);
        }
    }
}