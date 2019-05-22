using Cache.Core;
using Cache.Core.Api;
using Cache.Core.Implementation;
using Cache.Core.Model;
using ProductCatalogApi.Caches.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Caches {
    /// <summary>
    /// Product memory cache 
    /// </summary>
    public class ProductCache : IProductCacheProvider {
        private readonly int maxSize;
        private readonly int expireTime;
        ICacheProvider cacheProvider;
        /// <summary>
        /// Initialize maximum size of the cache and expire time 
        /// </summary>
        /// <param name="maxSize">Maximum size of the cache</param>
        /// <param name="expireTime">Expire time of items</param>
        public ProductCache(int maxSize, int expireTime) {
            this.maxSize = maxSize;
            this.expireTime = expireTime;
            BuildCache();

        }

        void BuildCache() {
            cacheProvider = Cache.Core.Api.Cache.Memory()
                       .MaximumSize(maxSize)
                       .ExpireAfterInactive(TimeSpan.FromSeconds(expireTime))
                       .Build();
        }
        /// <summary>
        /// Checks if the given key is in the cache.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> Exists(string key) {
            return await cacheProvider.Exists(key);
        }
        /// <summary>
        /// Gets product cache item from cache.
        /// </summary>
        /// <typeparam name="ProductCacheItem"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<ProductCacheItem> Pull<ProductCacheItem>(string key) {
            return await cacheProvider.Pull<ProductCacheItem>(key);
        }
        /// <summary>
        /// Puts item to the cache.
        /// </summary>
        /// <typeparam name="ProductCacheItem"></typeparam>
        /// <param name="cacheItem"></param>
        /// <returns></returns>
        public async Task Put<ProductCacheItem>(ICacheItem<ProductCacheItem> cacheItem) {
            await cacheProvider.Put(cacheItem);
        }
        /// <summary>
        /// Removes item from cache.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> Remove(string key) {
            return await cacheProvider.Remove(key);
        }
        /// <summary>
        /// Adds or updates the cache if exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheItem"></param>
        /// <returns></returns>
        public async Task PutOrUpdate<T>(ICacheItem<T> cacheItem) {
            string key = cacheItem.Key;
            bool isKeyExists = await cacheProvider.Exists(key);
            if (isKeyExists) {
                await cacheProvider.Remove(key);
            }
            await cacheProvider.Put(cacheItem);
        }
    }
}

