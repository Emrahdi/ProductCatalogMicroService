namespace ProductCatalogApi.Helpers {
    /// <summary>
    /// Configuration Keys
    /// </summary>
    public class AppSettings {

        /// <summary>
        /// Product Catalog connection key
        /// </summary>
        public const string ProductCatalogConnectionKey= "ProductCatalogConnection";
        /// <summary>
        /// Secret key used with jwt.
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// Can be used to enable/disable cache by passing this object to services(Product Service)
        /// </summary>
        public bool IsMemoryCachingEnabled { get; set; }
        /// <summary>
        /// Maximum size of the product cache
        /// </summary>
        public int ProductCacheMaxSize { get; set; }
        /// <summary>
        /// Expire time for product cache
        /// </summary>
        public int ProductCacheExpireTime { get; set; }
    }
}