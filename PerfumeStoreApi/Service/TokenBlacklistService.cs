using Microsoft.Extensions.Caching.Memory;
using PerfumeStoreApi.Service.Interfaces;

namespace PerfumeStoreApi.Service;

public class TokenBlacklistService : ITokenBlacklistService
{
    private readonly IMemoryCache _cache;

    public TokenBlacklistService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void RevokeToken(string jti, DateTimeOffset expiry)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = expiry
        };
        _cache.Set(GetCacheKey(jti), true, options);
    }

    public bool IsRevoked(string jti)
    {
        return _cache.TryGetValue(GetCacheKey(jti), out _);
    }

    private static string GetCacheKey(string jti) => $"blacklist:{jti}";
}
