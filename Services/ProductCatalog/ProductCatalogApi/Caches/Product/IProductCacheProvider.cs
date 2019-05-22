using Cache.Core;
using Cache.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Caches.Product {
    /// <summary>
    /// Could be used to add new features to product cache
    /// </summary>
    public interface IProductCacheProvider : ICacheProvider {

        /// <summary>
        /// Adds or updates(if exists) product info
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheItem"></param>
        /// <returns></returns>
        Task PutOrUpdate<T>(ICacheItem<T> cacheItem);
    }
}
