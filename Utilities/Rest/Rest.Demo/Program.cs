using System;
using System.Net.Http;
using System.Threading.Tasks;
using Rest.Core;
using Rest.Implementation;

namespace RestClientDemo {
    class Program {
        static void Main(string[] args) {
            //Get();

            IRestClientProvider restClientProvider = new DefaultRestClientProvider(10);
            restClientProvider = new CommonDecorationApplierClientProvider(restClientProvider);
            UserController userController = new UserController(restClientProvider);


            var userData = userController.GetAll().Result;

            var pagedData = userController.GetByPage(2).Result;

            User user = new User();
            user.name = "EmrahDi";
            user.job = "Develeper";

            var addedUser = userController.CreateUser(user).Result;

            Console.WriteLine(addedUser);
            Console.ReadLine();
        }

        private async static Task<T> Get<T>() {
            HttpClient client = new HttpClient();
            var xx = client.GetAsync("https://jsonplaceholder.typicode.com/todos/1");
            var yyy = await xx.Result.Content.ReadAsAsync<T>();

            return yyy;
        }
    }

}
