using System;
using System.Collections.Generic;
using System.Text;

namespace Cache.Core.Model {
    public class CacheItem<T> : ICacheItem<T> {

        public string Key { get; set; }
        public T Value { get; set; }

        public CacheItem(string key, T value) {
            this.Key = key;
            this.Value = value;
        }
    }
}
