using Cache.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Caches.Product {
    /// <summary>
    /// Product cache object item
    /// </summary>
    public class ProductCacheItem {

        /// <summary>
        /// Unique key of the product cache
        /// </summary>
        public string Key { get { return Id.ToString(); } }

        /// <summary>
        /// Product Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Product Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Product name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Product photo
        /// </summary>
        public byte[] Photo { get; set; }
        /// <summary>
        /// Product Price
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Last updated date of the product
        /// </summary>
        public DateTime LastUpdatedDate { get; set; }
        /// <summary>
        /// Last updating user code of the product
        /// </summary>
        public string LastUpdatedUser { get; set; }
    }
}
