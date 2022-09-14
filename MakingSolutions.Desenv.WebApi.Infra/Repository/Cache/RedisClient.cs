using MakingSolutions.Desenv.WebApi.Domain.Interfaces.Cache;
using MakingSolutions.Desenv.WebApi.Entities.Models;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace MakingSolutions.Desenv.WebApi.Infra.Repository.Cache
{
    public class RedisClient : ICacheClient, IDisposable
    {
        private readonly string _connectionString;
        private IConnectionMultiplexer _connectionMultiplexer;
        private IDatabase _database;
        private ISubscriber _subscriber;

        //public RedisClient(IOptions<ConnectionStrings> options)
        public RedisClient()
        {
            _connectionString = "localhost:6379";
            //_connectionString = options.Value.RedisCache;
            Connect();
        }
        public bool NotConfigured { get; private set; }

        public async Task<RedisValue> StringGetAsync(RedisKey redisKey)
        {
            if (NotConfigured)
            {
                return default;
            }

            try
            {
                return await _database.StringGetAsync(redisKey);
            }
            catch (RedisException)
            {
                return default;
            }
        }

        public async Task<IEnumerable<RedisValue>> StringGetAsync(RedisKey[] redisKey)
        {
            if (NotConfigured)
            {
                return default;
            }

            try
            {
                var result = await _database.StringGetAsync(redisKey);

                return result.Where(r => r.HasValue);
            }
            catch (RedisException)
            {
                return default;
            }
        }

        public async Task<bool?> StringSetAsync(RedisKey redisKey, RedisValue redisValue)
        {
            if (NotConfigured)
            {
                return default;
            }

            try
            {
                return await _database.StringSetAsync(redisKey, redisValue);
            }
            catch (RedisException)
            {
                return default;
            }
        }

        public async Task<bool?> StringSetAsync(KeyValuePair<RedisKey, RedisValue>[] values)
        {
            if (NotConfigured)
            {
                return default;
            }

            try
            {
                return await _database.StringSetAsync(values);
            }
            catch (RedisException)
            {
                return default;
            }
        }

        public async Task<bool?> KeyDeleteAsync(RedisKey redisKey)
        {
            if (NotConfigured)
            {
                return default;
            }

            try
            {
                return await _database.KeyDeleteAsync(redisKey);
            }
            catch (RedisException)
            {
                return default;
            }
        }

        public async Task<long?> KeyDeleteAsync(RedisKey[] redisKey)
        {
            if (NotConfigured)
            {
                return default;
            }

            try
            {
                return await _database.KeyDeleteAsync(redisKey);
            }
            catch (RedisException)
            {
                return default;
            }
        }

        public async Task<bool?> KeyExpireAsync(RedisKey redisKey, TimeSpan? timeSpan = default)
        {
            if (NotConfigured)
            {
                return default;
            }

            try
            {
                return await _database.KeyExpireAsync(redisKey, timeSpan);
            }
            catch (RedisException)
            {
                return default;
            }
        }

        public async Task<bool?> SubscribeAsync(RedisChannel redisChannel, Action<RedisChannel, RedisValue> handler)
        {
            if (NotConfigured)
            {
                return default;
            }

            try
            {
                await _subscriber.SubscribeAsync(redisChannel, handler);

                return true;
            }
            catch (RedisException)
            {
                return false;
            }
        }

        public async Task<bool?> UnsubscribeAsync(RedisChannel redisChannel)
        {
            if (NotConfigured)
            {
                return default;
            }

            try
            {
                await _subscriber.UnsubscribeAsync(redisChannel);

                return true;
            }
            catch (RedisException)
            {
                return false;
            }
        }

        private bool Connect()
        {
            try
            {
                _connectionMultiplexer = ConnectionMultiplexer.Connect(_connectionString);
                _database = _connectionMultiplexer.GetDatabase();
                _subscriber = _connectionMultiplexer.GetSubscriber();

                NotConfigured = _connectionMultiplexer is null;
                return true;
            }
            catch (Exception)
            {
                NotConfigured = _connectionMultiplexer is null;
                return false;
            }
        }

        public void Dispose()
        {
            _connectionMultiplexer.WaitAll();
            _connectionMultiplexer.Close(allowCommandsToComplete: true);
            _connectionMultiplexer.Dispose();
        }
    }
}
