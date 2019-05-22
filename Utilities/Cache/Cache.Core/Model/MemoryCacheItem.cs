using System;
using System.Collections.Generic;
using System.Text;

namespace Cache.Core.Model {
    public class MemoryCacheItem<T> : ICacheItem<T> {
        public string Key { get ; set ; }
        public T Value { get ; set ; }


        public MemoryCacheItem(string key, T value) {
            this.Key = key;
            this.Value = value;
        }
    }
}
