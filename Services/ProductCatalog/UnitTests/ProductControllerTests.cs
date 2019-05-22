using AutoMapper;
using Logger.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using ProductCatalog.DataLayer.Context;
using ProductCatalogApi.AutoMApper;
using ProductCatalogApi.Caches.Product;
using ProductCatalogApi.Controllers;
using ProductCatalogApi.Helpers;
using ProductCatalogApi.Models;
using ProductCatalogApi.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests {
    public class ProductControllerTests {
        private Mock<ILogger> iLogger;
        IMapper iMapper;
        ProductCatalogContext productCatalogContext;
        Mock<IOptions<AppSettings>> appSettings;
        Mock<IProductCacheProvider> productCacheService;
        HeaderModel headerDto;
        Mock<IUserService> userService;

        [SetUp]
        public void Setup() {

            iLogger = GetILoggerMockService();
            iMapper = GetAutoMapper();
            productCatalogContext = GetTestContext();
            appSettings = GetAppSettings();
            productCacheService = GetCacheProviderMockService();
            headerDto = new HeaderModel();
            headerDto.Authorization = "";
            userService = GetMockUserService();
        }

        [Test]
        public async Task Get() {

            ProductModel productModelResponse = new ProductModel { Code = "1", Id = 1, Name = "Product1", Price = 1 };
            ProductController productController = new ProductController(
                appSettings.Object,
                iMapper,
                productCatalogContext,
                productCacheService.Object,
                iLogger.Object,
                GetProductMockService(productModelResponse).Object,
                GetMockUserService().Object);

            var productResult1 = await Task.FromResult(productController.GetAsync("1"));
            Assert.IsInstanceOf<OkObjectResult>(productResult1.Result);
            var productObjectResult1 = productResult1.Result as OkObjectResult;
            Assert.AreEqual(productObjectResult1.StatusCode, 200);

            Assert.IsInstanceOf<ProductModel>(productObjectResult1.Value);
            var productModel1 = productObjectResult1.Value as ProductModel;
            Assert.AreEqual(productModel1.Name, productModelResponse.Name);

            string productCode2 = "999";
            var productResultNotFound = await Task.FromResult(productController.GetAsync(productCode2));
            Assert.IsInstanceOf<NotFoundObjectResult>(productResultNotFound.Result);
            var productObjectResultNotFound = productResultNotFound.Result as NotFoundObjectResult;
            Assert.AreEqual(productObjectResultNotFound.StatusCode, 404);

            Assert.IsInstanceOf<string>(productObjectResultNotFound.Value);
            var productModel2 = productObjectResultNotFound.Value as string;
            Assert.AreEqual(productCode2, productModel2);

        }

        [Test]
        public async Task Search() {

            ProductModel productModelResponse = new ProductModel { Code = "1", Id = 1, Name = "Product1", Price = 1 };
            ProductSearchModel productSearchModel = new ProductSearchModel() { Code = "LightSaber", Name = "LightSaber" };

            ProductController productController = new ProductController(
                appSettings.Object,
                iMapper,
                productCatalogContext,
                productCacheService.Object,
                iLogger.Object,
                GetProductMockService(productModelResponse).Object,
                GetMockUserService().Object);

            var productsResult1 = await Task.FromResult(productController.SearchProductsAsync("LightSaber", "LightSaber"));
            Assert.IsInstanceOf<OkObjectResult>(productsResult1.Result);
            var productObjectResult1 = productsResult1.Result as OkObjectResult;
            Assert.AreEqual(productObjectResult1.StatusCode, 200);

            Assert.IsInstanceOf<List<ProductModel>>(productObjectResult1.Value);
            var productsModel1 = productObjectResult1.Value as List<ProductModel>;
            Assert.Greater(productsModel1.Count, 0);

            var productResultNotFound = await Task.FromResult(productController.GetAsync("999"));
            Assert.IsInstanceOf<NotFoundObjectResult>(productResultNotFound.Result);
            var productObjectResultNotFound = productResultNotFound.Result as NotFoundObjectResult;
            Assert.AreEqual(productObjectResultNotFound.StatusCode, 404);
        }

        [Test]
        public async Task Save() {
            ProductModel productModelResponse = new ProductModel { Code = "1", Id = 1, Name = "Product1", Price = 1, RowStatus = "New" };
            ProductController productController = new ProductController(
               appSettings.Object,
               iMapper,
               productCatalogContext,
               productCacheService.Object,
               iLogger.Object,
               GetProductMockService(productModelResponse).Object,
               GetMockUserService().Object);
            var saveProductResult1 = await productController.SaveProductAsync(productModelResponse, headerDto);
            Assert.IsInstanceOf<OkObjectResult>(saveProductResult1);
            var productObjectResult1 = saveProductResult1 as OkObjectResult;
            Assert.AreEqual(productObjectResult1.StatusCode, 200);
            Assert.IsInstanceOf<ProductModel>(productObjectResult1.Value);
            var productModel1 = productObjectResult1.Value as ProductModel;
            Assert.AreEqual(productModel1.Name, productModelResponse.Name);

            productModelResponse.RowStatus = "Update";
            productModelResponse.Name = "Product1.1";
            var saveProductResult2 = await productController.SaveProductAsync(productModelResponse, headerDto);
            Assert.IsInstanceOf<OkObjectResult>(saveProductResult2);
            var productObjectResult2 = saveProductResult2 as OkObjectResult;
            Assert.AreEqual(productObjectResult2.StatusCode, 200);
            Assert.IsInstanceOf<ProductModel>(productObjectResult2.Value);
            var productModel2 = productObjectResult2.Value as ProductModel;
            Assert.AreEqual(productModel2.Name, productModelResponse.Name);


            productModelResponse.Name = "Product1.2";
            productModelResponse.Code = "999";
            var saveProductResultNotFound = await productController.SaveProductAsync(productModelResponse, headerDto);
            Assert.IsInstanceOf<NotFoundObjectResult>(saveProductResultNotFound);
            var productObjectResultNotFound = saveProductResultNotFound as NotFoundObjectResult;
            Assert.AreEqual(productObjectResultNotFound.StatusCode, 404);
            Assert.IsInstanceOf<ProductModel>(productObjectResultNotFound.Value);
            var productModelNotFound = productObjectResultNotFound.Value as ProductModel;
            Assert.AreEqual(productModelNotFound.Name, productModelResponse.Name);
        }

        [Test]
        public async Task Remove() {
            ProductModel productModelResponse = new ProductModel { Code = "1", Id = 1, Name = "Product1", Price = 1 };
            ProductDeleteRequestModel productDeleteRequestModel = new ProductDeleteRequestModel { Code = "1" };
            ProductController productController = new ProductController(
               appSettings.Object,
               iMapper,
               productCatalogContext,
               productCacheService.Object,
               iLogger.Object,
               GetProductMockService(productModelResponse).Object,
               GetMockUserService().Object);
            var mock = new Mock<ProductController>();
            var saveProductResult1 = await productController.RemoveProductAsync(productDeleteRequestModel, headerDto);
            Assert.IsInstanceOf<OkObjectResult>(saveProductResult1);
            var productObjectResult1 = saveProductResult1 as OkObjectResult;
            Assert.AreEqual(productObjectResult1.StatusCode, 200);

            productDeleteRequestModel.Code = "999";
            var saveProductResultNotFound = await productController.RemoveProductAsync(productDeleteRequestModel, headerDto);
            Assert.IsInstanceOf<NotFoundObjectResult>(saveProductResultNotFound);
            var productObjectResultNotFound = saveProductResultNotFound as NotFoundObjectResult;
            Assert.AreEqual(productObjectResultNotFound.StatusCode, 404);
        }

        [Test]
        public async Task SaveProductImage() {
            ProductModel productModelResponse = new ProductModel { Code = "1", Id = 1, Name = "Product1", Price = 1, RowStatus = "New" };
            ProductPhotoModel productPhotoModel = new ProductPhotoModel() { ProductCode = "1", PhotoStringFormat = "AAAABBBBBCCC" };
            ProductController productController = new ProductController(
               appSettings.Object,
               iMapper,
               productCatalogContext,
               productCacheService.Object,
               iLogger.Object,
               GetProductMockService(productModelResponse).Object,
               GetMockUserService().Object);
            var productImageResult = await productController.SaveProductImageAsync(productPhotoModel, headerDto);
            Assert.IsInstanceOf<OkResult>(productImageResult);
            var productObjectResult = productImageResult as OkResult;
            Assert.AreEqual(productObjectResult.StatusCode, 200);

            productPhotoModel.ProductCode = "999";
            var productImageResultWithNotFound = await productController.SaveProductImageAsync(productPhotoModel, headerDto);
            Assert.IsInstanceOf<NotFoundObjectResult>(productImageResultWithNotFound);
            var productObjectNotFoundResult = productImageResultWithNotFound as NotFoundObjectResult;
            Assert.AreEqual(productObjectNotFoundResult.StatusCode, 404);

        }
        Mock<IProductService> GetProductMockService(ProductModel productModel) {
            var mock = new Mock<IProductService>();
            mock.Setup(p => p.SaveProduct(productModel)).Returns(Task.FromResult(productModel));
            mock.Setup(p => p.SaveProduct(It.Is<ProductModel>(pp => pp.Code == "999"))).Throws<KeyNotFoundException>();
            mock.Setup(p => p.GetProduct(productModel.Code)).Returns(Task.FromResult(productModel));
            mock.Setup(p => p.GetProduct("999")).Throws<KeyNotFoundException>();
            mock.Setup(p => p.RemoveProduct(new ProductDeleteRequestModel() { Code = "1" }));
            mock.Setup(p => p.RemoveProduct(It.Is<ProductDeleteRequestModel>(pp => pp.Code == "999"))).Throws<KeyNotFoundException>();
            List<ProductModel> products = ProductTestData.GetTestData();
            mock.Setup(p => p.SearchProducts("LightSaber", "LightSaber")).Returns(Task.FromResult(products));
            mock.Setup(p => p.SearchProducts("999", It.IsAny<string>())).Throws<KeyNotFoundException>();
            mock.Setup(p => p.SaveProductImage(It.Is<ProductPhotoModel>(pp => pp.ProductCode == "1"))).Returns(Task.CompletedTask);
            mock.Setup(p => p.SaveProductImage(It.Is<ProductPhotoModel>(pp => pp.ProductCode == "999"))).Throws<KeyNotFoundException>();


            return mock;
        }
        Mock<ILogger> GetILoggerMockService() {
            var mock = new Mock<ILogger>();
            mock.Setup(p => p.Log(It.IsAny<Status>(), It.IsAny<string>(), It.IsAny<string>())).Callback(() => {
                System.Diagnostics.Debug.WriteLine("Test Log Message");
            });
            return mock;
        }
        Mock<IProductCacheProvider> GetCacheProviderMockService() {
            ProductCacheItem productCacheItem = new ProductCacheItem { Code = "1", Id = 1, Name = "Product1", Price = 1 };
            var mock = new Mock<IProductCacheProvider>();
            mock.Setup(p => p.Pull<ProductCacheItem>("1")).Returns(Task.FromResult(productCacheItem));
            mock.Setup(p => p.Exists("1")).Returns(Task.FromResult(false));
            return mock;
        }
        IMapper GetAutoMapper() {
            var mockMapper = new MapperConfiguration(cfg => {
                cfg.AddProfile(new AutoMapperProfile());
            });
            return mockMapper.CreateMapper();
        }
        ProductCatalogContext GetTestContext() {
            var options = new DbContextOptionsBuilder<ProductCatalogContext>()
              .UseInMemoryDatabase(databaseName: "ProductUnitTest")
              .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
              .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)) //Transactions are not supported by the in-memory store
              .Options;
            return new ProductCatalogContext(options);
        }
        Mock<IOptions<AppSettings>> GetAppSettings() {
            AppSettings appSettings = new AppSettings();
            appSettings.IsMemoryCachingEnabled = true;
            appSettings.ProductCacheExpireTime = 1000;
            appSettings.ProductCacheMaxSize = 100;
            appSettings.Secret = "This is the secret key,but must be kept in a vault! Not here!";
            var mock = new Mock<IOptions<AppSettings>>();
            mock.Setup(ap => ap.Value).Returns(appSettings);
            return mock;
        }
        Mock<IUserService> GetMockUserService() {
            var mock = new Mock<IUserService>();
            mock.Setup(p => p.GetExistingUserName(It.IsAny<HttpRequest>())).Returns("admin");
            return mock;
        }
    }
}
