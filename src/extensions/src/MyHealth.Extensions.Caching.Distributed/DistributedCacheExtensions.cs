using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace MyHealth.Extensions.Caching.Distributed
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default)
            where T : class
        {
            await distributedCache.SetAsync(key, JsonSerializer.SerializeToUtf8Bytes(value), options, token);
        }

        public static async Task<T> GetAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default)
            where T : class
        {
            var cached = await distributedCache.GetAsync(key, token);

            if (cached is null || cached.Length == 0)
                return null;

            return JsonSerializer.Deserialize<T>(cached);
        }
    }
}
