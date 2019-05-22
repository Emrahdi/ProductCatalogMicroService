using System;
using Cache.Core;
using Cache.Core.Decoration;
using Cache.Core.Implementation;

namespace Cache.Core.Api {
    public class DistributedCacheBuilder {

        private readonly DistributedCacheBuildModel distributedCacheBuildModel;

        public DistributedCacheBuilder(DistributedCacheBuildModel distributedCacheBuildModel) {
            this.distributedCacheBuildModel = distributedCacheBuildModel;
        }

        public DistributedCacheBuilder Host(string host) {
            distributedCacheBuildModel.Host = host;

            return this;
        }

        public DistributedCacheBuilder ExpireAfter(DateTime dateTime) {
            distributedCacheBuildModel.ExpireAfter = dateTime;

            return this;
        }

        public DistributedCacheBuilder ExpireAfterInactive(TimeSpan timeSpan) {
            distributedCacheBuildModel.ExpireAfterInactive = timeSpan;

            return this;
        }

        public ICacheProvider Build() {
            return new ExceptionDecorator(new DistributedCacheProvider(distributedCacheBuildModel));
        }
    }
}
