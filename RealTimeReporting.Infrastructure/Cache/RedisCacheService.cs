using RealTimeReporting.Domain.Interfaces;
using StackExchange.Redis;

namespace RealTimeReporting.Infrastructure.Cache
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _redisDb;

        public RedisCacheService(string connectionString)
        {
            try
            {
                var redis = ConnectionMultiplexer.Connect(connectionString);
                _redisDb = redis.GetDatabase();
            }
            catch (RedisConnectionException ex)
            {
                throw new Exception("Redis bağlantısı sağlanamadı.", ex);
            }
        }

        public async Task<string?> GetValueAsync(string key)
        {
            return await _redisDb.StringGetAsync(key);
        }

        public async Task SetValueAsync(string key, string value, TimeSpan? expiry = null)
        {
            await _redisDb.StringSetAsync(key, value, expiry);
        }
    }
}
