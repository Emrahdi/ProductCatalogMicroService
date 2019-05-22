using System;
using System.Collections.Generic;
using System.Text;

namespace Rest.Core {
    public class CommonDecorationApplierClientProvider : IRestClientProvider {
        private readonly IRestClientProvider decorated;

        public CommonDecorationApplierClientProvider(IRestClientProvider decorated) {
            this.decorated = decorated;
        }

        public IRestClient Provide(RestClientOptions restClientOptions) {
            IRestClient restClient = decorated.Provide(restClientOptions);
            restClient = new LogDecorator(restClient);
            restClient = new ExceptionDecorator(restClient);

            return restClient;
        }
    }
}
