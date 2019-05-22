using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cache.Core;
using Cache.Core.Model;

namespace Cache.Core.Implementation {
    public class DefaultCacheProvider : ICacheProvider {
        public Task<bool> Remove(string key) {
            throw new NotImplementedException();
        }

        public Task<bool> Exists(string key) {
            throw new NotImplementedException();
        }

        public Task<T> Pull<T>(string key) {
            throw new NotImplementedException();
        }

        public Task Put<T>(ICacheItem<T> cacheItem) {
            throw new NotImplementedException();
        }
    }
}
