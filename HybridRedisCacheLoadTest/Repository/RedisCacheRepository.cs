using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis;

namespace HybridRedisCacheLoadTest.Repository
{
    public class RedisCacheRepository : IRepository
    {
        private readonly IDatabase _db;
        public RedisCacheRepository(IDatabase db)
        {
            _db = db;
        }
        public async ValueTask SetValueAsync(int item, string msg)
        {
            await _db.StringSetAsync(item.ToString(), msg + item);
        }

        public async ValueTask GetValueAsync(int item, string msg)
        {
            var value = await _db.StringGetAsync(item.ToString());
            var actual = msg + item.ToString();
            Assert.AreEqual<string>(actual, value);
        }

        public async ValueTask SetAndGetValueAsync(int item, string message)
        {
            await _db.StringSetAsync(item.ToString(), message);
            var result = await _db.StringGetAsync(item.ToString());
            Assert.AreEqual<string>(message, result);
        }

        public Task SetValueAsync(string key, string value)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetValueAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task ClearAll()
        {
            throw new NotImplementedException();
        }

        public Task SetValueAsync(Dictionary<string, string> value)
        {
            throw new NotImplementedException();
        }
    }
}
