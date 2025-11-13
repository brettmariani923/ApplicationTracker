using Microsoft.Data.SqlClient;
using System.Data;
using ApplicationTracker.Data.Interfaces;

namespace ApplicationTracker.Data.Implementation
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString) => _connectionString = connectionString;

        public IDbConnection NewConnection() => new SqlConnection(_connectionString);
    }
}
