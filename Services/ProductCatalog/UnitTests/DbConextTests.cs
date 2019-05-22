using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProductCatalog.DataLayer.Context;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;


namespace UnitTests {

    public class DbConextTests {
        [Test]
        public void ProductDbOperationsOperations() {
            var options = new DbContextOptionsBuilder<ProductCatalogContext>()
                .UseInMemoryDatabase(databaseName: "ProductUnitTest")
               .Options;
            using (ProductCatalogContext ctx = new ProductCatalogContext(options)) {
                var result = ctx.Product.Where(p => p.Code == "MilleniumFalcon").ToList();
                Assert.AreEqual(result.Count, 0);
                var products = GetTestData();
                ctx.Product.AddRange(products);
                ctx.SaveChanges();
                Assert.AreEqual(products.Count, ctx.Product.Count());
                var bowCasterProduct = ctx.Product.Where(p => p.Code == "bowCaster").ToList().FirstOrDefault();
                Assert.AreEqual(bowCasterProduct.Name, "Bow Caster");
                bowCasterProduct.Name = "MultiBowCaster";
                ctx.Product.Update(bowCasterProduct);
                ctx.SaveChanges();
                var getProductResult2 = ctx.Product.Where(p => p.Code == "bowCaster").ToList().FirstOrDefault();
                Assert.AreEqual(getProductResult2.Name, "MultiBowCaster");
                ctx.Product.Remove(getProductResult2);
                ctx.SaveChanges();
                var finalProductResult = ctx.Product.Find(getProductResult2.Id);
                Assert.AreEqual(finalProductResult, null);
            }
        }

        List<Product> GetTestData() {
            List<Product> products = new List<Product>();
            Product p1 = new Product() {
                Code = "redLightSaber1",
                Name = "Red Light Saber",
                LastUpdatedDate = DateTime.Now,
                Price = 15,
                LastUpdatedUser = "Darth Vader"
            };
            Product p2 = new Product() {
                Code = "greenLightSaber1",
                Name = "Green Light Saber",
                LastUpdatedDate = DateTime.Now,
                Price = 10,
                LastUpdatedUser = "Yoda"
            };
            Product p3 = new Product() {
                Code = "bowCaster",
                Name = "Bow Caster",
                LastUpdatedDate = DateTime.Now,
                Price = 5,
                LastUpdatedUser = "Chewbacca"
            };
            products.Add(p1);
            products.Add(p2);
            products.Add(p3);
            return products;
        }

    }
}
