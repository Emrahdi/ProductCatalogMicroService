using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rest.Core {
    public interface IRestClient {

        Task<T> PostAsync<T>(string requestUri, object requestData);

        Task<T> GetAsync<T>(string requestUri);

        Task<T> GetAsync<T>(string requestUri, IDictionary<string, string> parameters);
    }
}
