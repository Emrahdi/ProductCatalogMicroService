using System;

namespace Cache.Core.Api {
    public class DistributedCacheBuildModel {

        public string Host { get; set; }

        public DateTime? ExpireAfter { get; set; }

        public TimeSpan? ExpireAfterInactive { get; set; }
    }
}
