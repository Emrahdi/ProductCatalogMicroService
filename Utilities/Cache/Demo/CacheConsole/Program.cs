using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using Cache.Core.Model;

namespace CacheConsole {
    class Program {
        static void Main(string[] args) {
            Memory();
            //Redis();
        }

        

        private static void Redis() {
            var cache = Cache.Core.Api.Cache.Distributed()
                            .Host("localhost:6379")
                            .ExpireAfterInactive(TimeSpan.FromSeconds(2))
                            .Build();


            cache.Put(new CacheItem<string>("testDist", "test1"));

            var value = cache.Pull<string>("testDist").Result;

            Thread.Sleep(2000);

            var value2 = cache.Pull<string>("testDist").Result;

        }

        private static void Memory() {
            var cache = Cache.Core.Api.Cache.Memory()
                            .MaximumSize(100)
                            .ExpireAfterInactive(TimeSpan.FromSeconds(10000))
                            .Build();

            cache.Put(new CacheItem<string>("test", "test1"));
            var value = cache.Pull<string>("test").Result;
            Thread.Sleep(2000);
            var value2 = cache.Pull<string>("test").Result;
        }
    }

}
