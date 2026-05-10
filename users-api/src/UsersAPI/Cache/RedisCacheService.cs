using Microsoft.Extensions.Caching.Distributed;

namespace UsersAPI.Cache;

public class RedisCacheService : ICacheService
{
  private readonly IDistributedCache _cache;

  public RedisCacheService(
      IDistributedCache cache)
  {
    _cache = cache;
  }

  public async Task<string> GetOrCreateAsync(string key)
  {
    var cached =
        await _cache.GetStringAsync(key);

    if (!string.IsNullOrEmpty(cached))
    {
      return $"redis:{cached}";
    }

    var value =
        $"generated-{DateTime.UtcNow}";

    await _cache.SetStringAsync(
        key,
        value,
        new DistributedCacheEntryOptions
        {
          AbsoluteExpirationRelativeToNow =
                TimeSpan.FromMinutes(5)
        });

    return $"api:{value}";
  }
}