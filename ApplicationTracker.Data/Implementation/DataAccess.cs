using Dapper;
using System.Data;
using ApplicationTracker.Data.Interfaces;


namespace ApplicationTracker.Data.Implementation
{
    public class DataAccess : IDataAccess
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public DataAccess(IDbConnectionFactory connectionFactory) => _connectionFactory = connectionFactory;
        public async Task<int> ExecuteAsync(IDataExecute request) => await HandleRequest(async _ => await _.ExecuteAsync(request.GetSql(), request.GetParameters()));
        public async Task<TResponse?> FetchAsync<TResponse>(IDataFetch<TResponse> request) => await HandleRequest(async _ => await _.QueryFirstOrDefaultAsync<TResponse>(request.GetSql(), request.GetParameters()));
        public async Task<IEnumerable<TResponse>> FetchListAsync<TResponse>(IDataFetchList<TResponse> request) => await HandleRequest(async _ => await _.QueryAsync<TResponse>(request.GetSql(), request.GetParameters()));
    
        //helper
        private async Task<T> HandleRequest<T>(Func<IDbConnection, Task<T>> funcHandleRequest)
        {
            using var connection = _connectionFactory.NewConnection();

            connection.Open();

            return await funcHandleRequest.Invoke(connection);
        }
    }
}