using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalogApi.DependencyInjection.AutoFac;
using ProductCatalogApi.DependencyInjection.AutoFacModules;
using System;

namespace ProductCatalogApi.DependencyInjection {

    /// <summary>
    /// Extension service class for DI. Can be used if there is a entry point to the application except Controller.
    /// Ex: A thread running which is triggered at the startUp of the application.
    /// </summary>
    public static class ProductServiceExtensions {
        static IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Added services can be used from di container 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceProvider GenerateProductProvider(this IServiceCollection services, IConfiguration configuration) {

            IProductBuilder builder = new ProductBuilder(services, configuration);
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            containerBuilder.RegisterModule(new ProductModule());
            containerBuilder.RegisterModule(new UserModule());
            var container = containerBuilder.Build();
            ServiceProvider = new AutofacServiceProvider(container);
            return ServiceProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInstance<T>() {
            return ServiceProvider.GetService<T>();
        }
    }
}
