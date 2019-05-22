using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rest.Core {
    public class LogDecorator : IRestClient {
        private readonly IRestClient decorated;
        
        public LogDecorator(IRestClient decorated) {
            this.decorated = decorated;
        }

        public async Task<T> GetAsync<T>(string requestUri) {
            try {
                return await decorated.GetAsync<T>(requestUri);
            }
            catch (Exception exception) {
                //Log
                throw exception;
            }
        }

        public async Task<T> GetAsync<T>(string requestUri, IDictionary<string, string> parameters) {
            try {
                return await decorated.GetAsync<T>(requestUri, parameters);
            }
            catch (Exception exception) {
                //Log
                throw exception;
            }
        }

        public async Task<T> PostAsync<T>(string requestUri, object requestData) {
            try {
                return await decorated.PostAsync<T>(requestUri, requestData);
            }
            catch (Exception exception) {
                //Log
                throw exception;
            }
        }
    }
}
