using System;
using Cache.Core;
using Cache.Core.Decoration;
using Cache.Core.Implementation;

namespace Cache.Core.Api {
    public class MemoryCacheBuilder {

        private readonly MemoryCacheBuildModel memoryCache;

        public MemoryCacheBuilder(MemoryCacheBuildModel memoryCache) {
            this.memoryCache = memoryCache;
        }

        public MemoryCacheBuilder MaximumSize(long size) {
            memoryCache.MaximumSize = size;

            return this;
        }

        public MemoryCacheBuilder ExpireAfter(DateTime dateTime) {
            memoryCache.ExpireAfter = dateTime;

            return this;
        }

        public MemoryCacheBuilder ExpireAfterInactive(TimeSpan timeSpan) {
            memoryCache.ExpireAfterInactive = timeSpan;

            return this;
        }

        public ICacheProvider Build() {
            return new ExceptionDecorator(new MemoryCacheProvider(memoryCache));
        }
    }
}
