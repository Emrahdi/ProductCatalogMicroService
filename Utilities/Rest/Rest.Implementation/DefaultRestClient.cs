using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Rest.Core;

namespace Rest.Implementation {
    public class DefaultRestClient : IRestClient {
        private readonly HttpClient client;

        public DefaultRestClient(HttpClient client) {
            this.client = client;
        }

        public async Task<T> GetAsync<T>(string requestUri) {
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var val = await response.Content.ReadAsAsync<T>();

            return val;
        }

        public async Task<T> GetAsync<T>(string requestUri, IDictionary<string, string> parameters) {
            if (parameters == null || parameters.Count == 0) {
                return await GetAsync<T>(requestUri);
            }

            string requestUrlWithParameter = BuildUrlWithParameter(requestUri, parameters);

            var response = await client.GetAsync(requestUrlWithParameter);
            response.EnsureSuccessStatusCode();
            var val = await response.Content.ReadAsAsync<T>();

            return val;
        }

        public async Task<T> PostAsync<T>(string requestUri, object requestData) {
            var response = await client.PostAsJsonAsync(requestUri, requestData);
            response.EnsureSuccessStatusCode();
            T val = await response.Content.ReadAsAsync<T>();

            return val;
        }

        private static string BuildUrlWithParameter(string requestUri, IDictionary<string, string> parameters) {
            StringBuilder builder = new StringBuilder(requestUri);
            builder.Append("?");
            foreach (var item in parameters) {
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                builder.Append("&");
            }
            builder.Remove(builder.Length - 1, 1);
            string result = builder.ToString();

            return result;
        }
    }
}
