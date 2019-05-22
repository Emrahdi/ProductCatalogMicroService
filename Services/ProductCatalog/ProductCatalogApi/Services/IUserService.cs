using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Services {
    /// <summary>
    /// User operations service
    /// </summary>
    public interface IUserService {

        /// <summary>
        /// Get user name of the request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        string GetExistingUserName(HttpRequest request);
    }
}
