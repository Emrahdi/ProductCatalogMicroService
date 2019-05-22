using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Models {
    /// <summary>
    /// Product object used by product controller api.
    /// </summary>
    public partial class ProductModel {
        /// <summary>
        /// Product Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Product Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Product Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Product Photo
        /// </summary>
        public byte[] Photo { get; set; }
        /// <summary>
        /// Product Price
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Last update date of the transaction
        /// </summary>
        public DateTime LastUpdatedDate { get; set; }
        /// <summary>
        /// Last updating user code of the transaction
        /// </summary>
        public string LastUpdatedUser { get; set; }
    }

    /// <summary>
    /// Product Model
    /// </summary>
    public partial class ProductModel {
        /// <summary>
        /// initializes rowstatus as no change
        /// </summary>
        public ProductModel() {
            RowStatus = "NoChange";
        }
        /// <summary>
        /// Row status of the record. (UI table determines if a record is new or updated)
        /// </summary>
        [NotMapped]
        public string RowStatus { get; set; }
        /// <summary>
        /// Image in string format
        /// </summary>
        public string PhotoStringFormat { get; set; }
    }
}
