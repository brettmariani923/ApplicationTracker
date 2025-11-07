namespace ApplicationTracker.Data.Interfaces
{
    internal interface IDataRequest
    {
        public string GetSql();
        public object? GetParameters();
    }
}
