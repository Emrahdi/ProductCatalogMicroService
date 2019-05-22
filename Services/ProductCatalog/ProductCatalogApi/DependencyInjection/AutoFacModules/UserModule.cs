using Autofac;
using ProductCatalogApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.DependencyInjection.AutoFacModules {
    /// <summary>
    /// User service di initializer
    /// </summary>
    public class UserModule : Module {
        /// <summary>
        /// Register user service as autofac module to DI container.
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder) {

            builder.Register(c => new UserService()).As<IUserService>();
        }
    }
}
