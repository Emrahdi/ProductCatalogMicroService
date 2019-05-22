using System;
using System.Collections.Generic;
using System.Text;

namespace Cache.Core.Model {
    public class DistributedCacheItem<T> : ICacheItem<T> {
        public string Key { get; set; }
        public T Value { get; set; }
    }
}
