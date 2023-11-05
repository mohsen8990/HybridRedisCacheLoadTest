namespace RedisTest1.Repository
{
    public interface IRepository
    {
        Task SetValueAsync(string key, string value);
        Task<string> GetValueAsync(string key);
        Task ClearAll();
        Task SetValueAsync(Dictionary<string, string> value);
    }
}