using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Models
{
    /// <summary>
    /// Header information used to accept authorization fields within the apis.
    /// </summary>
    public class HeaderModel {
        /// <summary>
        /// Authorization info
        /// </summary>
        public string Authorization { get; set; }
    }
}
