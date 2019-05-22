using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cache.Core;
using Cache.Core.Model;
using Microsoft.Extensions.Caching.Memory;
using Cache.Core.Api;

namespace Cache.Core.Implementation {
    public class MemoryCacheProvider : ICacheProvider {

        private readonly IMemoryCache memoryCache;
        private readonly MemoryCacheBuildModel memoryCacheBuildModel;

        public MemoryCacheProvider(MemoryCacheBuildModel memoryCacheBuildModel) {
            MemoryCacheOptions memoryCacheOptions = new MemoryCacheOptions();
            memoryCacheOptions.SizeLimit = memoryCacheBuildModel.MaximumSize;

            this.memoryCache = new MemoryCache(memoryCacheOptions);
            this.memoryCacheBuildModel = memoryCacheBuildModel;
        }

        public async Task<bool> Exists(string key) {
            object value;
            bool result = memoryCache.TryGetValue(key, out value);
            return await Task.FromResult(result);
        }

        public async Task<T> Pull<T>(string key) {
            T value;
            memoryCache.TryGetValue(key, out value);

            return await Task.FromResult(value);
        }

        public async Task Put<T>(ICacheItem<T> cacheItem) {
            memoryCache.Set(cacheItem.Key, cacheItem.Value, new MemoryCacheEntryOptions() {
                Size = 1,
                AbsoluteExpiration = memoryCacheBuildModel.ExpireAfter,
                SlidingExpiration = memoryCacheBuildModel.ExpireAfterInactive
            });
            await Task.Yield();
        }

        public async Task<bool> Remove(string key) {
            object value;
            bool result = memoryCache.TryGetValue(key, out value);
            if (result) {
                memoryCache.Remove(key.ToString());
            }
            return await Task.FromResult(true);
        }
    }
}
