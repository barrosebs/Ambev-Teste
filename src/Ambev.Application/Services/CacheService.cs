using Microsoft.Extensions.Caching.Memory;
using System;

namespace Ambev.Application.Services
{
    public interface ICacheService
    {
        T? GetOrSet<T>(string key, Func<T> getItemCallback, TimeSpan? expiration = null) where T : class;
        void Remove(string key);
        void Clear();
    }

    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(30);

        public CacheService(IMemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public T? GetOrSet<T>(string key, Func<T> getItemCallback, TimeSpan? expiration = null) where T : class
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("A chave não pode ser nula ou vazia.", nameof(key));

            if (getItemCallback == null)
                throw new ArgumentNullException(nameof(getItemCallback));

            if (_cache.TryGetValue(key, out T? cachedItem))
            {
                return cachedItem;
            }

            var item = getItemCallback();
            if (item == null)
                return null;

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(expiration ?? _defaultExpiration)
                .SetPriority(CacheItemPriority.Normal);

            _cache.Set(key, item, cacheEntryOptions);
            return item;
        }

        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("A chave não pode ser nula ou vazia.", nameof(key));

            _cache.Remove(key);
        }

        public void Clear()
        {
            if (_cache is MemoryCache memoryCache)
            {
                memoryCache.Compact(1.0);
            }
        }
    }
} 