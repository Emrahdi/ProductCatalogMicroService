using System;
using System.Collections.Generic;
using System.Text;

namespace Cache.Core.Api {
    public class Cache {

        public static MemoryCacheBuilder Memory() {
            MemoryCacheBuildModel memoryCache = new MemoryCacheBuildModel();
            return new MemoryCacheBuilder(memoryCache);
        }

        public static DistributedCacheBuilder Distributed() {
            DistributedCacheBuildModel distributedCacheBuildModel = new DistributedCacheBuildModel();
            return new DistributedCacheBuilder(distributedCacheBuildModel);
        }
    }
}
