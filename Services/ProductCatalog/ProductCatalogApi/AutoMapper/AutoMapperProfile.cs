using AutoMapper;
using ProductCatalog.DataLayer.Context;
using ProductCatalogApi.Caches.Product;
using ProductCatalogApi.Models;

namespace ProductCatalogApi.AutoMApper {
    /// <summary>
    /// automaps objects
    /// </summary>
    public class AutoMapperProfile : Profile {
        /// <summary>
        /// Ctor that creates maps of objects for automapper.
        /// </summary>
        public AutoMapperProfile() {
            CreateMap<Product, ProductModel>().ReverseMap();
            CreateMap<Product, ProductCacheItem>().ReverseMap();
            CreateMap<ProductModel, ProductCacheItem>().ReverseMap();
            CreateMap<Product, ProductPhotoModel>()
               .ForMember(d => d.ProductCode, opt => opt.MapFrom(s => s.Code))
               .ReverseMap();
        }
    }
}