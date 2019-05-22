using ProductCatalogApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Services {
    /// <summary>
    /// Product operations service
    /// </summary>
    public interface IProductService {

        /// <summary>
        /// Get product info by product code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<ProductModel> GetProduct(string code);

        ///// <summary>
        ///// Get product by id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //Task<ProductModel> GetProductById(int id);

        /// <summary>
        /// Search products according to product code or name
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<List<ProductModel>> SearchProducts(string code, string name);

        /// <summary>
        /// Returns top {count} products
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<List<ProductModel>> TakeProducts(int count);

        /// <summary>
        /// Save product
        /// </summary>
        /// <param name="productModel"></param>
        Task<ProductModel> SaveProduct(ProductModel productModel);

        /// <summary>
        /// Removes the given object
        /// </summary>
        /// <param name="productDeleteRequestModel"></param>
        /// <returns></returns>
        Task RemoveProduct(ProductDeleteRequestModel productDeleteRequestModel);

        /// <summary>
        /// Save photo by given product code
        /// </summary>
        /// <param name="productPhotoModel"></param>
        Task SaveProductImage(ProductPhotoModel productPhotoModel);

    }
}
