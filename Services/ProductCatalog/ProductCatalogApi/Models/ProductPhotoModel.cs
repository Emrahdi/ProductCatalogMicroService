using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Models
{
    /// <summary>
    /// Product photo model used for product image operation
    /// </summary>
    public class ProductPhotoModel
    {
        /// <summary>
        /// Product Code
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Photo data in string format
        /// </summary>
        public string PhotoStringFormat { get; set; }
        /// <summary>
        /// Last updating user code
        /// </summary>
        public string LastUpdatedUser { get; set; }
    }
}
