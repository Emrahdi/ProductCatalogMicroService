using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cache.Core;
using Cache.Core.Model;

namespace Cache.Core.Decoration {
    public class ExceptionDecorator : ICacheProvider {
        private readonly ICacheProvider decorated;

        public ExceptionDecorator(ICacheProvider decorated) {
            this.decorated = decorated;
        }

        public async Task<bool> Exists(string key) {
            try {
                return await decorated.Exists(key);
            }
            catch {
                throw;
            }
        }

        public async Task<T> Pull<T>(string key) {
            try {
                return await decorated.Pull<T>(key);
            }
            catch {
                throw;
            }
        }

        public async Task Put<T>(ICacheItem<T> cacheItem) {
            try {
                await decorated.Put<T>(cacheItem);
            }
            catch {
                throw;
            }
        }

        public async Task<bool> Remove(string key) {
            try {
                await decorated.Remove(key);
            }
            catch {
                throw;
            }
            return true;
        }
    }
}
