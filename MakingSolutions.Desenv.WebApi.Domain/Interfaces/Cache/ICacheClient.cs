using StackExchange.Redis;

namespace MakingSolutions.Desenv.WebApi.Domain.Interfaces.Cache
{
    public interface ICacheClient
    {
        Task<bool?> StringSetAsync(RedisKey redisKey, RedisValue redisValue);
    }
}
