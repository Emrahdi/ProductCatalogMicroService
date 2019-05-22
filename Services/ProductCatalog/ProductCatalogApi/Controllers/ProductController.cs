using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProductCatalogApi.Helpers;
using Microsoft.Extensions.Options;
using log4net;
using System.Reflection;
using ProductCatalogApi.Models;
using ProductCatalog.DataLayer.Context;
using ProductCatalogApi.Caches.Product;
using System.Threading.Tasks;
using AutoMapper;
using Logger.Core;
using ProductCatalogApi.Services;

namespace ProductCatalogApi.Controllers {

    /// <summary>
    /// Product api
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    public class ProductController : Controller {

        private readonly AppSettings appSettings;
        private readonly IMapper mapper;
        private readonly ProductCatalogContext productCatalogcontext;
        private readonly IProductCacheProvider productCacheProvider;
        private readonly ILogger logger;
        private readonly IProductService productService;
        private readonly IUserService userService;

        /// <summary>
        /// Product controller ctor gets services from DI container.
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="mapper"></param>
        /// <param name="productCatalogcontext"></param>
        /// <param name="productCacheProvider"></param>
        /// <param name="logger"></param>
        /// <param name="productService"></param>
        /// <param name="userService"></param>
        public ProductController(
           IOptions<AppSettings> appSettings,
           IMapper mapper,
           ProductCatalogContext productCatalogcontext,
           IProductCacheProvider productCacheProvider,
           ILogger logger,
           IProductService productService,
           IUserService userService) {
            this.appSettings = appSettings.Value;
            this.mapper = mapper;
            this.productCatalogcontext = productCatalogcontext;
            this.productCacheProvider = productCacheProvider;
            this.logger = logger;
            this.productService = productService;
            this.userService = userService;
        }

        /// <summary>
        /// Get product info by product code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("GetAsync")]
        public async Task<IActionResult> GetAsync(string code) {

            ProductModel product = null;
            try {
                product = await productService.GetProduct(code);
            }
            catch (KeyNotFoundException keyNotFoundException) {
                logger.Log(Status.Warn, string.Format("Get-ErrorMessage:{0},StackTrace:{1}", keyNotFoundException.Message, keyNotFoundException.StackTrace));
                return NotFound(code);
            }
            catch (Exception ex) {
                logger.Log(Status.Error, string.Format("Get-ErrorMessage:{0},StackTrace:{1}", ex.Message, ex.StackTrace));
                return StatusCode(500);
            }
            return Ok(product);
        }

        /// <summary>
        /// Searchs the products according to code and name.If inputs are not valid, takes top 5 elements.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("SearchProductsAsync")]
        public async Task<IActionResult> SearchProductsAsync(string code, string name) {
            List<ProductModel> products;
            try {
                if (string.IsNullOrEmpty(code) && string.IsNullOrEmpty(name)) {
                    products = await productService.TakeProducts(5);
                }
                else {
                    try {
                        products = await productService.SearchProducts(code, name);
                    }
                    catch (KeyNotFoundException keyNotFoundException) {
                        logger.Log(Status.Error, string.Format("SearchProducts-ErrorMessage:{0},StackTrace:{1}", keyNotFoundException.Message, keyNotFoundException.StackTrace));
                        return NotFound(code);
                    }
                }
            }
            catch (Exception ex) {
                logger.Log(Status.Error, string.Format("SearchProducts-ErrorMessage:{0},StackTrace:{1}", ex.Message, ex.StackTrace));
                return StatusCode(500);
            }
            return Ok(products);
        }

        /// <summary>
        /// Saves product info
        /// </summary>
        /// <param name="productModel"></param>
        /// <param name="headerDto"></param>
        /// <returns></returns>
        [HttpPost("SaveProductAsync")]
        public async Task<IActionResult> SaveProductAsync([FromBody]ProductModel productModel, [FromHeader] HeaderModel headerDto) {
            ProductModel productModelResult = productModel;
            if (!ModelState.IsValid) {
                logger.Log(Status.Error, string.Format("SaveProduct-{0}", GetValidationErrors()));
                return ValidationProblem();
            }
            productModel.LastUpdatedUser = userService.GetExistingUserName(Request);
            try {
                productModelResult= await productService.SaveProduct(productModel);
            }
            catch (KeyNotFoundException keyNotFoundException) {
                logger.Log(Status.Error, string.Format("SaveProduct-ErrorMessage:{0},StackTrace:{1}!Product Code:{2}", keyNotFoundException.Message,
                    keyNotFoundException.StackTrace, productModel.Code));
                return NotFound(productModel);
            }
            catch (Exception ex) {
                logger.Log(Status.Error, string.Format("SaveProduct-ErrorMessage:{0},StackTrace:{1}!Product Code:{2}", ex.Message,
                    ex.StackTrace, productModel.Code));
                return StatusCode(500);
            }
            return Ok(productModelResult);
        }

        /// <summary>
        /// Deletes product
        /// </summary>
        /// <param name="productDeleteRequestModel"></param>
        /// <param name="headerDto"></param>
        /// <returns></returns>
        [HttpDelete("RemoveProductAsync")]
        public async Task<IActionResult> RemoveProductAsync([FromBody]ProductDeleteRequestModel productDeleteRequestModel, [FromHeader] HeaderModel headerDto) {
            if (!ModelState.IsValid) {
                logger.Log(Status.Error, string.Format("RemoveProduct-{0}", GetValidationErrors()));
                return ValidationProblem();
            }
            try {
                await productService.RemoveProduct(productDeleteRequestModel);
            }
            catch (KeyNotFoundException keyNotFoundException) {
                logger.Log(Status.Error, string.Format("RemoveProduct-ErrorMessage:{0},StackTrace:{1}!Product Code:{2}", keyNotFoundException.Message,
                    keyNotFoundException.StackTrace, productDeleteRequestModel.Code));
                return NotFound(productDeleteRequestModel);
            }
            catch (Exception ex) {
                logger.Log(Status.Error, string.Format("RemoveProduct-ErrorMessage:{0},StackTrace:{1}!Product Code:{2}", ex.Message,
                                    ex.StackTrace, productDeleteRequestModel.Code));
                return StatusCode(500);
            }
            return Ok(productDeleteRequestModel);

        }

        /// <summary>
        /// Save product image
        /// </summary>
        /// <param name="productPhotoModel"></param>
        /// <param name="headerDto"></param>
        /// <returns></returns>
        [HttpPost("SaveProductImageAsync")]
        public async Task<IActionResult> SaveProductImageAsync([FromBody]ProductPhotoModel productPhotoModel, [FromHeader] HeaderModel headerDto) {
            if (!ModelState.IsValid) {
                logger.Log(Status.Error, string.Format("SaveProductImage-{0}", GetValidationErrors()));
                return ValidationProblem();
            }
            productPhotoModel.LastUpdatedUser = userService.GetExistingUserName(Request);
            try {
                await productService.SaveProductImage(productPhotoModel);
            }
            catch (KeyNotFoundException keyNotFoundException) {
                logger.Log(Status.Error, string.Format("SaveProductImage-ErrorMessage:{0},StackTrace:{1}!Product Code:{2}", keyNotFoundException.Message,
                    keyNotFoundException.StackTrace, productPhotoModel.ProductCode));
                return NotFound(productPhotoModel);
            }
            catch (Exception ex) {
                logger.Log(Status.Error, string.Format("SaveProductImage-ErrorMessage:{0},StackTrace:{1}!Product Code:{2}", ex.Message,
                                    ex.StackTrace, productPhotoModel.ProductCode));
                return StatusCode(500);
            }
            await productService.SaveProductImage(productPhotoModel);
            return Ok();
        }
        /// <summary>
        /// Get validation errors from model state
        /// </summary>
        /// <returns></returns>
        string GetValidationErrors() {
            string errorResults = string.Empty;
            if (ModelStateHelper.TryGetValidationErrorsConcataneted(ModelState, out errorResults)) {
                return errorResults;
            }
            logger.Log(Status.Error, "Validation errors can not be read!");
            return errorResults;
        }
    }
}

