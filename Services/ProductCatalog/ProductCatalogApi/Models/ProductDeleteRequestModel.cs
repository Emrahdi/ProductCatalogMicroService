using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Models
{
    /// <summary>
    /// Product delete request object
    /// </summary>
    public class ProductDeleteRequestModel
    {
        /// <summary>
        /// Product Code
        /// </summary>
        public string Code { get; set; }
    }
}
