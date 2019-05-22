using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Rest.Core;

namespace Rest.Implementation {
    public class DefaultRestClientProvider : IRestClientProvider {
        private readonly int timeOutInSeconds;

        public DefaultRestClientProvider(int timeOutInSeconds) {
            this.timeOutInSeconds = timeOutInSeconds;
        }

        public IRestClient Provide(RestClientOptions restClientOptions) {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = restClientOptions.BaseUri;
            httpClient.Timeout = new TimeSpan(0, 0, timeOutInSeconds);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(restClientOptions.MediaType));

            IRestClient result = new DefaultRestClient(httpClient);
            
            return result;
        }
    }
}
