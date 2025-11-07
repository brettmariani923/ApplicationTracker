namespace ApplicationTracker.Data.Interfaces
{
    public interface IDataRequest
    {
        public string GetSql();
        public object? GetParameters();
    }
}
