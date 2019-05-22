using AutoMapper;
using Cache.Core.Model;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.DataLayer.Context;
using ProductCatalogApi.Caches.Product;
using ProductCatalogApi.Helpers;
using ProductCatalogApi.Models;
using ProductCatalogApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Services {

    /// <summary>
    /// Product operations using dbcontext and also memory cache Mechanisim.
    /// </summary>
    public class ProductService : IProductService {
        private readonly IProductCacheProvider productCacheProvider;
        private readonly IMapper mapper;
        private readonly ProductCatalogContext productCatalogContext;

        /// <summary>
        /// Ctor with injecting types
        /// </summary>
        /// <param name="productCacheProvider"></param>
        /// <param name="mapper"></param>
        /// <param name="productCatalogContext"></param>
        public ProductService(IProductCacheProvider productCacheProvider, IMapper mapper, ProductCatalogContext productCatalogContext) {
            this.productCacheProvider = productCacheProvider;
            this.mapper = mapper;
            this.productCatalogContext = productCatalogContext;
        }

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ProductModel> GetProduct(string code) {
            Product product = await GetProductByCode(code);
            return mapper.Map<ProductModel>(product);
        }

        /// <summary>
        /// Searchs products
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<ProductModel>> SearchProducts(string code, string name) {

            List<ProductModel> productsResult = new List<ProductModel>();
            if (await productCacheProvider.Exists(code)) {
                //Note:If there are more than 1 server, the products could be saved through other servers.Cache should be triggered and updated for all servers.
                //Trigger mechanism should be implemented for cache!!!
                //Ex: product "X" is saved with productMicroservice1. SearchProducts is routed to productMicroservice2. productMicroservice2 cache doesn't contain data with product "X"
                var productCacheItem = await productCacheProvider.Pull<ProductCacheItem>(name);
                ProductModel productModel = mapper.Map<ProductModel>(productCacheItem);
                productModel.PhotoStringFormat = BytesHelper.GetString(productModel.Photo);
                productsResult.Add(productModel);
                return productsResult;
            }
            else {
                var products = await productCatalogContext.Product.Where(p => p.Code == code
                                                                         || p.Name == name).ToListAsync();
                if (products == null) {
                    throw new KeyNotFoundException(string.Format("Product is not Found! Product Code:{0} , Name:{1}", code, name));
                    //https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-4.0/ms229021(v=vs.100)
                }
                var productCacheItems = mapper.Map<List<ProductCacheItem>>(products);
                foreach (var p in productCacheItems) {
                    await productCacheProvider.Put(new CacheItem<ProductCacheItem>(p.Code, p));
                }
                return mapper.Map<List<ProductModel>>(productCacheItems);
            }
        }

        /// <summary>
        /// Take top {0} products
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<List<ProductModel>> TakeProducts(int count) {
            var products = await productCatalogContext.Product.Take(count).ToListAsync();
            List<ProductModel> productModels = mapper.Map<List<ProductModel>>(products);
            productModels.ForEach(x => x.PhotoStringFormat = BytesHelper.GetString(x.Photo));
            return await Task.FromResult(productModels);
        }

        /// <summary>
        /// Saves product according to to rowstatus parameter
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        public async Task<ProductModel> SaveProduct(ProductModel productModel) {
            var product = mapper.Map<Product>(productModel);
            if (productModel.RowStatus == "New") {
                await AddProduct(product);
            }
            else {
                //var result = await GetProductByCode(productModel.Code);
                await UpdateProduct(product);
            }
             return mapper.Map<ProductModel>(product);
        }

        /// <summary>
        /// Saves product image of an existing product
        /// </summary>
        /// <param name="productPhotoModel"></param>
        /// <returns></returns>
        public async Task SaveProductImage(ProductPhotoModel productPhotoModel) {
            Product product = await GetProductByCode(productPhotoModel.ProductCode);
            product.Photo = BytesHelper.GetUTFBytes(productPhotoModel.PhotoStringFormat);
            product.LastUpdatedDate = DateTime.Now;
            product.LastUpdatedUser = productPhotoModel.LastUpdatedUser;
            try {
                productCatalogContext.Database.BeginTransaction();
                productCatalogContext.Product.Update(product);
                await productCatalogContext.SaveChangesAsync();
                productCatalogContext.Database.CommitTransaction();
                productCatalogContext.Entry(product).State = EntityState.Detached;
            }
            catch {
                productCatalogContext.Database.RollbackTransaction();
                throw;
            }
        }
        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="productDeleteRequestModel"></param>
        /// <returns></returns>
        public async Task RemoveProduct(ProductDeleteRequestModel productDeleteRequestModel) {
            Product product = await GetProductByCode(productDeleteRequestModel.Code);
            if (product == null) {
                throw new KeyNotFoundException("Product is not found!");
            }
            try {
                productCatalogContext.Database.BeginTransaction();
                productCatalogContext.Product.Remove(product);
                await productCatalogContext.SaveChangesAsync();
                productCatalogContext.Database.CommitTransaction();
                productCatalogContext.Entry(product).State = EntityState.Detached;
            }
            catch {
                productCatalogContext.Database.RollbackTransaction();
                throw;
            }
        }
        /// <summary>
        /// Update existing product info
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private async Task UpdateProduct(Product product) {
            try {
                product.LastUpdatedDate = DateTime.Now;
                productCatalogContext.Database.BeginTransaction();
                productCatalogContext.Product.Update(product);
                await productCatalogContext.SaveChangesAsync();
                productCatalogContext.Database.CommitTransaction();
                product = productCatalogContext.Entry(product).Entity;
                productCatalogContext.Entry(product).State = EntityState.Detached; //When using instance of context(unit testing), EF Core tracks the object. I should be singleton or deatached.
            }
            catch {
                productCatalogContext.Database.RollbackTransaction();
                throw;
            }
            await productCacheProvider.Remove(product.Code);
        }
        /// <summary>
        /// Adds a new product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private async Task AddProduct(Product product) {
            try {
                product.LastUpdatedDate = DateTime.Now;
                productCatalogContext.Database.BeginTransaction();
                product.Id = 0;
                productCatalogContext.Product.Add(product);
                await productCatalogContext.SaveChangesAsync();
                productCatalogContext.Database.CommitTransaction();
                product = productCatalogContext.Entry(product).Entity;
                productCatalogContext.Entry(product).State = EntityState.Detached; //When using instance of context(unit testing), EF Core tracks the object. I should be singleton or deatached.
            }
            catch {
                productCatalogContext.Database.RollbackTransaction();
                throw;
            }
            await Task.Yield();
        }

        /// <summary>
        /// Get product by product code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task<Product> GetProductByCode(string code) {
            //Note: Autofac DI container also can be used for injected types.
            //var mapper = ProductServiceExtensions.GetInstance<IMapper>();
            //var productCatalogContext = ProductServiceExtensions.GetInstance<ProductCatalogContext>().Product;
            if (await productCacheProvider.Exists(code.ToString())) {
                var productCacheItem = await productCacheProvider.Pull<ProductCacheItem>(code.ToString());
                Product product = mapper.Map<Product>(productCacheItem);
                return product;
            }
            else {
                var product = await productCatalogContext.Product.FirstOrDefaultAsync(p => p.Code == code);
                if (product == null) {
                    throw new KeyNotFoundException(string.Format("Product is not Found! Product Code:{0}", code));
                    //https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-4.0/ms229021(v=vs.100)
                }
                var productCacheItem = mapper.Map<ProductCacheItem>(product);
                await productCacheProvider.Put(new CacheItem<ProductCacheItem>(code.ToString(), productCacheItem));
                return mapper.Map<Product>(product);
            }
        }

        /// <summary>
        /// Get product by product id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<Product> GetProductById(int id) {
            var product = await productCatalogContext.Product.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) {
                throw new KeyNotFoundException(string.Format("Product is not Found! Product Id:{0}", id));
                //https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-4.0/ms229021(v=vs.100)
            }
            var productCacheItem = mapper.Map<ProductCacheItem>(product);
            await productCacheProvider.Put(new CacheItem<ProductCacheItem>(productCacheItem.Code.ToString(), productCacheItem));
            return mapper.Map<Product>(product);
        }

    }
}
