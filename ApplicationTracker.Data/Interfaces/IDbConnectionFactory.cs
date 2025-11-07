using System.Data;

namespace ApplicationTracker.Data.Interfaces
{
    public interface IDbConnectionFactory
    {
        public IDbConnection NewConnection();
    }
}
