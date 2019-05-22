using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rest.Core {
    public class DefaultBackendRestClient : IBackendRestClient {

        private readonly IRestClient restClient;

        public DefaultBackendRestClient(IRestClientProvider restClientProvider, string backendUrl, int connectionTimeOut) {
            RestClientOptions restClientOptions = new RestClientOptions();
            restClientOptions.BaseUri = new Uri(backendUrl);
            restClientOptions.ConnectionTimeOut = connectionTimeOut;

            this.restClient = restClientProvider.Provide(restClientOptions);
        }

        public async Task<T> GetAsync<T>(string requestUri) {
            return await restClient.GetAsync<T>(requestUri);
        }

        public async Task<T> GetAsync<T>(string requestUri, IDictionary<string, string> parameters) {
            return await restClient.GetAsync<T>(requestUri, parameters);
        }

        public async Task<T> PostAsync<T>(string requestUri, object requestData) {
            return await restClient.PostAsync<T>(requestUri, requestData);
        }
    }
}
