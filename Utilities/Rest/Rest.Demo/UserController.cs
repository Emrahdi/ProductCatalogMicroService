using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Rest.Core;

namespace RestClientDemo {
    public class UserController {
        private readonly IRestClient restClient;

        public UserController(IRestClientProvider restClientProvider) {
            RestClientOptions restClientOptions = new RestClientOptions();
            restClientOptions.BaseUri = new Uri("https://reqres.in/");
            restClientOptions.ConnectionTimeOut = 100;
            restClientOptions.MediaType = "text/html";
            this.restClient = restClientProvider.Provide(restClientOptions);
        }

        public async Task<UserData> GetAll() {
            var result = await restClient.GetAsync<UserData>("api/users");

            return result;
        }

        public async Task<UserData> GetByPage(int page) {
            IDictionary<string, string> pairs = new Dictionary<string, string>() {
                {"page", page.ToString() }
            };
            var result = await restClient.GetAsync<UserData>("api/users", pairs);

            return result;
        }

        public async Task<User> CreateUser(User user) {
            var result = await restClient.PostAsync<User>("api/users", user);

            return result;
        }
    }
}
