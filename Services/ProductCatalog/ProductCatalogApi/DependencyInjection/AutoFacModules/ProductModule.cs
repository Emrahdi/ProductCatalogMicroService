using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductCatalog.DataLayer.Context;
using ProductCatalogApi.Caches.Product;
using ProductCatalogApi.Services;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalogApi.Helpers;

namespace ProductCatalogApi.DependencyInjection.AutoFac {

    /// <summary>
    /// Product service di initializer
    /// </summary>
    public class ProductModule : Module {

        /// <summary>
        /// Register product service as autofac module to DI container.
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder) {

            builder.Register(c => {
                var config = c.Resolve<IConfiguration>();
                var opt = new DbContextOptionsBuilder<ProductCatalogContext>().UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                opt.UseSqlServer(config.GetConnectionString(AppSettings.ProductCatalogConnectionKey));
                return new ProductService(c.Resolve<IProductCacheProvider>(),
                    c.Resolve<IMapper>(),
                    new ProductCatalogContext(opt.Options));
            }).As<IProductService>();
        }
    }
}
