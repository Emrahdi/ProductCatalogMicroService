using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProductCatalog.DataLayer.Context;
using Microsoft.EntityFrameworkCore;
using ProductCatalogApi.Services;
using ProductCatalogApi.Caches;
using Moq;
using ProductCatalogApi.Caches.Product;
using System.Threading.Tasks;
using AutoMapper;
using ProductCatalogApi.AutoMApper;
using ProductCatalogApi.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using System.Threading;

namespace UnitTests {
    public class ProductServiceTests {
        [Test]
        public async Task SaveTest() {
            IProductService productService = new ProductService(GetCacheProviderMockService().Object, GetAutoMapper(), GetTestContext());
            ProductModel productModel1 = new ProductModel() { Code = "1", Id = 1, Name = "Product1", Price = 1, RowStatus = "New" };
            ProductModel productModel2 = new ProductModel() { Code = "2", Id = 2, Name = "Product2", Price = 2, RowStatus = "New" };
            ProductModel productModel3 = new ProductModel() { Code = "3", Id = 3, Name = "Product3", Price = 3, RowStatus = "New" };
            ProductModel productModel4 = new ProductModel() { Code = "4", Id = 4, Name = "Product4", Price = 4, RowStatus = "New" };
            ProductModel productModel5 = new ProductModel() { Code = "5", Id = 5, Name = "Product5", Price = 5, RowStatus = "New" };
            await productService.SaveProduct(productModel1);
            await productService.SaveProduct(productModel2);
            await productService.SaveProduct(productModel3);
            await productService.SaveProduct(productModel4);
            await productService.SaveProduct(productModel5);
        }

        [Test]
        public async Task SaveAndGetTest() {
            IProductService productService = new ProductService(GetCacheProviderMockService().Object, GetAutoMapper(), GetTestContext());
            ProductModel productModel1 = new ProductModel() { Code = "1", Id = 1, Name = "Product1", Price = 1, RowStatus = "New" };
            await productService.SaveProduct(productModel1);

            var productGetResult = Task.FromResult(await productService.GetProduct("1"));
            Assert.IsTrue(productGetResult.IsCompletedSuccessfully);
            Assert.NotNull(productGetResult.Result);
            Assert.AreEqual(productGetResult.Result.Code, "1");

        }

        [Test]
        public async Task SaveAndGetAndUpdateTest() {
            IProductService productService = new ProductService(GetCacheProviderMockService().Object, GetAutoMapper(), GetTestContext());
            ProductModel productModel1 = new ProductModel() { Code = "1", Id = 1, Name = "Product1", Price = 1, RowStatus = "New" };
            var productGetResult =await productService.SaveProduct(productModel1);
            decimal price = 2;
            productGetResult.Price = price;
            productGetResult.RowStatus = "Update";
            await productService.SaveProduct(productGetResult);

            var productGetResultAfterUpdate = Task.FromResult(await productService.GetProduct("1"));
            Assert.IsTrue(productGetResultAfterUpdate.IsCompletedSuccessfully);
            Assert.NotNull(productGetResultAfterUpdate.Result);
            Assert.AreEqual(productGetResultAfterUpdate.Result.Code, "1");
        }

        [Test]
        public async Task TakeTest() {
            IProductService productService = new ProductService(GetCacheProviderMockService().Object, GetAutoMapper(), GetTestContext());
            ProductModel productModel1 = new ProductModel() { Code = "1", Id = 1, Name = "Product1", Price = 1, RowStatus = "New" };
            ProductModel productModel2 = new ProductModel() { Code = "2", Id = 2, Name = "Product2", Price = 2, RowStatus = "New" };
            ProductModel productModel3 = new ProductModel() { Code = "3", Id = 3, Name = "Product3", Price = 3, RowStatus = "New" };
            ProductModel productModel4 = new ProductModel() { Code = "4", Id = 4, Name = "Product4", Price = 4, RowStatus = "New" };
            ProductModel productModel5 = new ProductModel() { Code = "5", Id = 5, Name = "Product5", Price = 5, RowStatus = "New" };
            await productService.SaveProduct(productModel1);
            await productService.SaveProduct(productModel2);
            await productService.SaveProduct(productModel3);
            await productService.SaveProduct(productModel4);
            await productService.SaveProduct(productModel5);

            int topCount = 4;
            var topProductsResult = Task.FromResult(await productService.TakeProducts(topCount));
            Assert.IsTrue(topProductsResult.IsCompletedSuccessfully);
            Assert.NotNull(topProductsResult.Result);
            Assert.AreEqual(topProductsResult.Result.Count, topCount);

        }

        [Test]
        public async Task RemoveTest() {
            IProductService productService = new ProductService(GetCacheProviderMockService().Object, GetAutoMapper(), GetTestContext());
            ProductModel productModel1 = new ProductModel() { Code = "1", Id = 1, Name = "Product1", Price = 1, RowStatus = "New" };
            await productService.SaveProduct(productModel1);

            await productService.RemoveProduct(new ProductDeleteRequestModel() { Code = "1" });
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await productService.GetProduct("1"));
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await productService.GetProduct("999"));
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
    }

}

