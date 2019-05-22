using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Logger.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using ProductCatalog.DataLayer.Context;
using ProductCatalogApi.Caches;
using ProductCatalogApi.Caches.Product;
using ProductCatalogApi.Helpers;
using ProductCatalogApi.Models;
using ProductCatalogApi.Services;
using ProductCatalogApi.Validation.Product;
using Swashbuckle.AspNetCore.Swagger;

namespace ProductCatalogApi.DependencyInjection {
    /// <summary>
    /// Di initializer class 
    /// </summary>
    public class ProductBuilder : IProductBuilder {

        /// <summary>
        /// DI service collection
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Configuration properties
        /// </summary>
        public IConfiguration Configuration { get; }
        const string appSettingsSectionKey = "AppSettings";
        AppSettings appSettings { get; set; }
        /// <summary>
        /// ctor initializes all the services and configures appsettings
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public ProductBuilder(IServiceCollection services, IConfiguration configuration) {
            Services = services;
            Configuration = configuration;
            AddAppSettings();
            Services.AddCors();
            Services.AddMvc().AddFluentValidation();
            AddValidators();
            Services.AddAutoMapper();
            AddDbContext();
            AddJwt();
            AddSwaggerService();
            Services.AddSingleton<IProductCacheProvider>(new ProductCache(appSettings.ProductCacheMaxSize, appSettings.ProductCacheExpireTime));
            Services.AddSingleton<ILogger>(new ConsoleLogger());
            RegisterProductServices();
        }
        void RegisterProductServices() {
            // is injected using autoFacModule
            //Services.AddScoped<IProductService>(s => new ProductService(s.GetService<IProductCacheProvider>(),
            //s.GetService<IMapper>(),
            //s.GetService<ProductCatalogContext>()));
        }
        void AddDbContext() {
            Services.AddDbContext<ProductCatalogContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString(AppSettings.ProductCatalogConnectionKey)));
        }
        void AddSwaggerService() {

            Services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1",
                    new Info {
                        Title = "Product Catalog",
                        Version = "v1",
                        Description = "Product Catalog Microservice",
                        Contact = new Contact {
                            Name = "Emrah Dinçadam",
                            Email = "emrahdincadam@gmail.com"
                        }
                    });

                string applicationBasePath =
                    PlatformServices.Default.Application.ApplicationBasePath;
                string applicationName =
                    PlatformServices.Default.Application.ApplicationName;
                string productCatalogXmlDoc =
                    System.IO.Path.Combine(applicationBasePath, $"{applicationName}.xml");
                c.IncludeXmlComments(productCatalogXmlDoc);
            });
        }
        void AddJwt() {
            var key = BytesHelper.GetASCIIBytes(appSettings.Secret);
            Services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
        void AddValidators() {
            Services.AddTransient<IValidator<ProductSearchModel>, ProductSearchValidator>();
            Services.AddTransient<IValidator<ProductModel>, ProductSaveValidator>();
            Services.AddTransient<IValidator<ProductPhotoModel>, ProductPhotoValidator>();
            Services.AddTransient<IValidator<ProductDeleteRequestModel>, ProductDeleteValidation>();
        }
        void AddAppSettings() {
            var appSettingsSection = Configuration.GetSection(appSettingsSectionKey);
            Services.Configure<AppSettings>(appSettingsSection);
            appSettings = appSettingsSection.Get<AppSettings>();
        }
        void AddProductCacheProvider() {
            int maxSize = appSettings.ProductCacheMaxSize;
            int expireTime = appSettings.ProductCacheExpireTime;
            Services.AddSingleton<IProductCacheProvider>(new ProductCache(maxSize, expireTime));
        }
    }
}
