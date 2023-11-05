using HybridRedisCache;
using HybridRedisCacheLoadTest.Repository;
using LiteDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HybridRedisCacheLoadTest
{
    public class LoadTest
    {


        private const string SetAndGetKey = @"Key2";
        private const string SetAndGetValue = @"Hello this is test .";

        public async Task TestSetValue()
        {
            using var connection = ConnectionBuilder.CreateHybridCache(@"172.23.166.72:6379");
            var db = new HybridCacheRepository(connection);
            await db.SetValueAsync(SetAndGetKey, SetAndGetValue);
        }

        public async Task TestGetValue()
        {
            using var connection = ConnectionBuilder.CreateHybridCache(@"172.23.166.71:6379");
            var db = new HybridCacheRepository(connection);
            var value = await db.GetValueAsync(SetAndGetKey);
            Assert.AreEqual<string>(value, SetAndGetValue);
        }


        public async Task TestReadStorage()
        {
            using var db = new LiteDatabase(DataBaseName);
            var collection = db.GetCollection<IdModel>(CollectionName);
            var keys = collection.Query().ToList();
            using var connection = ConnectionBuilder.CreateHybridCache();
            var totalCount = keys.Count;
            var notFunded = 0;
            
            foreach (var key in keys)
            {
                var cacheValue = await connection.GetAsync<string>(key.Id);
                if (string.IsNullOrEmpty(cacheValue) || !cacheValue.ToString().Contains(key.Id))
                    notFunded++;
            }

            Console.WriteLine($"Total {totalCount}  Not Found {notFunded}");
            Console.WriteLine("Completed !!!");
        }


        const string DataBaseName = @"db.db";
        const string CollectionName = @"ItemKeys";
        const string RedisConnection = @"172.23.166.71:6379";

        public async Task TestStorage()
        {
            const int threadCount = 150000000;
            for (var i = 0; i < threadCount; i++)
            {
                Console.WriteLine($"Thread {i + 1} started !");
                Insert(i);
            }

            Console.WriteLine("Completed !!!");

            async Task Insert(int threadNumber)
            {
                try
                {
                    const int itemCount = 30000;
                    var result = new List<string>();

                    using var connection = ConnectionBuilder.CreateHybridCache();

                    for (var i = 0; i < itemCount; i++)
                    {
                        var key = Guid.NewGuid().ToString();
                        var value = GetValue(key);
                        await connection.SetAsync<string>(key, value,
                            new HybridCacheEntry(TimeSpan.FromDays(7), TimeSpan.FromDays(7), true, false, true));
                        result.Add(key);
                    }
                    
                    WriteResult(threadNumber, result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            async Task WriteResult(object number, List<string> keys)
            {
                lock (number)
                {
                    using var db = new LiteDatabase(DataBaseName);
                    var collection = db.GetCollection<IdModel>(CollectionName);
                    collection.InsertBulk(keys.Select(x => new IdModel() { Id = x }).ToList());
                    db.Commit();
                }
            }

            string GetValue(string key)
            {
                var result = "";
                const int len = 100;
                for (int i = 0; i < len ; i++)
                {
                    result = $"{result}{key}";
                }

                return result;
            }
        }

        public async Task ClearAll()
        {
            using var connection = ConnectionBuilder.CreateHybridCache();
            await connection.ClearAllAsync();
            Console.WriteLine("Completed !!!");
        }

        public async Task AddKey(string? key , string? value)
        {
            using var connection = ConnectionBuilder.CreateHybridCache();
            await connection.SetAsync(key, value , new HybridCacheEntry()
            {
                LocalCacheEnable = true , 
                RedisCacheEnable = true ,
                LocalExpiry = TimeSpan.FromDays(1),
                RedisExpiry = TimeSpan.FromDays(1)
            });
            Console.WriteLine("Completed !!!");
        }

        public async Task GetKey(string? key)
        {
            using var connection = ConnectionBuilder.CreateHybridCache();
            var value = await connection.GetAsync<string>(key);
            Console.WriteLine($"{value} : Completed !!!");
        }

        public async Task RemoveKey(string? key)
        {
            using var connection = ConnectionBuilder.CreateHybridCache();
            await connection.RemoveAsync(key);
            Console.WriteLine("Completed !!!");
        }

    }


    public class IdModel
    {
        public string Id { get; set; }
    }

}