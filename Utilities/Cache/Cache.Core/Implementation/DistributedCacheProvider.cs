using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cache.Core;
using Cache.Core.Model;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Cache.Core.Api;
using Microsoft.Extensions.Caching.Redis;

namespace Cache.Core.Implementation {
    public class DistributedCacheProvider : ICacheProvider {

        private readonly IDistributedCache distributedCache;
        private readonly DistributedCacheBuildModel distributedCacheBuildModel;

        public DistributedCacheProvider(DistributedCacheBuildModel distributedCacheBuildModel) {
            RedisCacheOptions redisCacheOptions = new RedisCacheOptions();
            redisCacheOptions.Configuration = distributedCacheBuildModel.Host;

            this.distributedCache = new RedisCache(redisCacheOptions);
            this.distributedCacheBuildModel = distributedCacheBuildModel;
        }

        public async Task<bool> Exists(string key) {
            var result = await distributedCache.GetAsync(key);

            return result != null;
        }

        public async Task<T> Pull<T>(string key) {
            var cacheValueBytes = await distributedCache.GetAsync(key);
            var serializedValue = Encoding.UTF8.GetString(cacheValueBytes);
            var result = JsonConvert.DeserializeObject<T>(serializedValue);

            return result;
        }

        public async Task Put<T>(ICacheItem<T> cacheItem) {
            string serializedValue = JsonConvert.SerializeObject(cacheItem.Value);
            var valueBytes = Encoding.UTF8.GetBytes(serializedValue);

            await distributedCache.SetAsync(cacheItem.Key, valueBytes, new DistributedCacheEntryOptions() {
                AbsoluteExpiration = distributedCacheBuildModel.ExpireAfter,
                SlidingExpiration = distributedCacheBuildModel.ExpireAfterInactive
            });
        }

        public async Task<bool> Remove(string key) {
            var result = await distributedCache.GetAsync(key);
            if (result == null) {
                return true;
            }
            try {
                distributedCache.Remove(key);
            }
            catch {
                return false;
            }
            return true;
        }

    }
}
