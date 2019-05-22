using System;
using System.Collections.Generic;
using ProductCatalog.DataLayer.Context;
using ProductCatalogApi.Caches.Product;
using ProductCatalogApi.Models;

namespace UnitTests {
    public static class ProductTestData {
        public static List<ProductModel> GetTestData() {
            List<ProductModel> products = new List<ProductModel>();
            ProductModel p1 = new ProductModel() {
                Code = "redLightSaber1",
                Name = "Red Light Saber",
                LastUpdatedDate = DateTime.Now,
                Price = 15,
                LastUpdatedUser = "Darth Vader"
            };
            ProductModel p2 = new ProductModel() {
                Code = "greenLightSaber1",
                Name = "Green Light Saber",
                LastUpdatedDate = DateTime.Now,
                Price = 10,
                LastUpdatedUser = "Yoda"
            };
            ProductModel p3 = new ProductModel() {
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
