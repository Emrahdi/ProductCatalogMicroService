using System;
using System.Collections.Generic;
using System.Text;

namespace Rest.Core {
    public interface IRestClientProvider {

        IRestClient Provide(RestClientOptions restClientOptions);
    }

    public class RestClientOptions {

        public Uri BaseUri { get; set; }

        public int ConnectionTimeOut { get; set; }

        public string MediaType { get; set; }
    }
}
