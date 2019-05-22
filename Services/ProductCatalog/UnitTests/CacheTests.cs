using System;
using Cache.Core.Model;
using System.Threading;
using ProductCatalogApi.Caches.Product;
using System.Collections.Generic;
using NUnit.Framework;
using Cache.Core.Api;
using System.Threading.Tasks;

namespace UnitTests {

    public class CacheTests {
        [Test]
        public async Task SimpleCacheTest() {
            var cache = Cache.Core.Api.Cache.Memory()
                           .MaximumSize(100)
                           .ExpireAfterInactive(TimeSpan.FromSeconds(10000))
                           .Build();
            await cache.Put(new CacheItem<string>("key1", "data1"));
            await cache.Put(new CacheItem<string>("key2", "data2"));
            await cache.Put(new CacheItem<string>("key3", "data3"));
            await cache.Put(new CacheItem<string>("key4", "data4"));
            var key1 = await cache.Pull<string>("key1");
            Thread.Sleep(2000);
            var key11 = await cache.Pull<string>("key1");
            Assert.AreEqual(key1, key11);
            var removeResult = await cache.Remove("key1");
            key1 = await cache.Pull<string>("key1");
            Assert.IsNull(key1);
        }

        [Test]
        public async Task ProductCacheTest() {
            var cache = Cache.Core.Api.Cache.Memory()
                           .MaximumSize(100)
                           .ExpireAfterInactive(TimeSpan.FromSeconds(10000))
                           .Build();

            var productData = GetCacheTestData();
            foreach (var pData in productData) {
                await cache.Put(new CacheItem<ProductCacheItem>(pData.Code, pData));
            }
            var firstPull = await cache.Pull<string>("redLightSaber1");
            Thread.Sleep(2000);
            var secondPull = await cache.Pull<string>("redLightSaber1");
            Assert.AreEqual(firstPull, secondPull);

            var removeResult = await cache.Remove("redLightSaber1");
            var keyRemove = await cache.Pull<string>("redLightSaber1");
            Assert.IsNull(keyRemove);

        }

        List<ProductCacheItem> GetCacheTestData() {
            List<ProductCacheItem> products = new List<ProductCacheItem>();
            ProductCacheItem p1 = new ProductCacheItem() {
                Code = "redLightSaber1",
                Name = "Red Light Saber",
                LastUpdatedDate = DateTime.Now,
                Price = 15,
                LastUpdatedUser = "Darth Vader"
            };
            ProductCacheItem p2 = new ProductCacheItem() {
                Code = "greenLightSaber1",
                Name = "Green Light Saber",
                LastUpdatedDate = DateTime.Now,
                Price = 10,
                LastUpdatedUser = "Yoda"
            };
            ProductCacheItem p3 = new ProductCacheItem() {
                Code = "bowCaster",
                Name = "Bow Caster",
                LastUpdatedDate = DateTime.Now,
                Price = 5,
                LastUpdatedUser = "Chewbacca"
            };
            products.Add(p1);
            products.Add(p2);
            products.Add(p3);
            return products;
        }
    }

}
