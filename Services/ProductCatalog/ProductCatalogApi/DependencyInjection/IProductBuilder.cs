using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProductCatalogApi.DependencyInjection {
    /// <summary>
    /// Project builder interface used to initialize services and configuration at the start of the application
    /// </summary>
    public interface IProductBuilder {

        /// <summary>
        /// DI service collection
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// Configuration properties
        /// </summary>
        IConfiguration Configuration { get; }

    }
}
