using HybridRedisCache;
using StackExchange.Redis;

namespace RedisTest1
{
    public static class ConnectionBuilder
    {
        const string ServersAddress =
            @"172.23.44.61:6379";

        public static IDatabase CreateInstanceConnection()
        {
            IDatabase db =
                GetRedisDatabase(ServersAddress);
            return db;
        }

        public static IDatabase GetRedisDatabase(string connection)
        {
            ConfigurationOptions configuration = ConfigurationOptions.Parse(connection);
            configuration.AbortOnConnectFail = false;
            configuration.AllowAdmin = true;
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configuration);
            return redis.GetDatabase();
        }


        public static HybridCache CreateHybridCache(string address = null)
        {
            var options = new HybridCachingOptions()
            {
                DefaultLocalExpirationTime = TimeSpan.FromMinutes(1),
                DefaultDistributedExpirationTime = TimeSpan.FromDays(1),
                InstancesSharedName = "SampleApp",
                ThrowIfDistributedCacheError = true,
                RedisConnectString = string.IsNullOrEmpty(address) ? ServersAddress : address,
                EnableLogging = true,
                FlushLocalCacheOnBusReconnection = true
            };
            return new HybridCache(options);
        }
    }
}
