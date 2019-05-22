using System;

namespace Cache.Core.Api {
    public class MemoryCacheBuildModel {
        public long MaximumSize { get; set; }

        public DateTime? ExpireAfter { get; set; }

        public TimeSpan? ExpireAfterInactive { get; set; }
    }
}
