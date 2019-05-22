using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Models {
    /// <summary>
    /// Product search request class
    /// </summary>
    public class ProductSearchModel {

        /// <summary>
        /// Product code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Product name
        /// </summary>
        public string Name { get; set; }

    }
}
