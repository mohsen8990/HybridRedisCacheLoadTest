using HybridRedisCache;

namespace HybridRedisCacheLoadTest.Repository
{
    public class HybridCacheRepository : IRepository
    {
        private readonly HybridCache _db;
        public HybridCacheRepository(HybridCache db)
        {
                _db = db;
        }
        public async Task<string> GetValueAsync(string key)
        {
            return await _db.GetAsync<string>(key);
        }

        public async Task ClearAll()
        {
            await _db.ClearAllAsync();
        }

        public async Task SetValueAsync(string key, string value)
        {

            await _db.SetAsync(key , value);
        }

        public async Task SetValueAsync(Dictionary<string , string> value)
        {
            await _db.SetAllAsync<string>(value , new HybridCacheEntry(TimeSpan.FromHours(12) , TimeSpan.FromHours(12) , false , false , true));
        }

    }
}
