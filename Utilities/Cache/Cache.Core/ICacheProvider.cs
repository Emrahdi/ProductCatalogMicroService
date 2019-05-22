using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cache.Core.Model;

namespace Cache.Core {
    public interface ICacheProvider {

        Task Put<T>(ICacheItem<T> cacheItem);

        Task<T> Pull<T>(string key);

        Task<bool> Exists(string key);

        Task<bool> Remove(string key);
    }
}
