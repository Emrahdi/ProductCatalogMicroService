using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Services {
    /// <summary>
    /// User service operations
    /// </summary>
    public class UserService : IUserService {
        /// <summary>
        /// Gets user name from http context.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetExistingUserName(HttpRequest request) {
            return ((Microsoft.AspNetCore.Http.DefaultHttpContext)((Microsoft.AspNetCore.Http.Internal.DefaultHttpRequest)request).HttpContext).User.Identities.First().Name;
        }
    }
}
