using System;
using Logger.Core;
using Logger.Implementation.RabitMQ;

namespace RabitMqConsumerDemo {
    class Program {
        static void Main(string[] args) {
            string rabitMqHost = "localhost";
            string rabitUserName = "admin";
            string rabitPassowrd = "password";

            ILogger consoleLogger = new ConsoleLogger();

            IRabitMQLogConsumer rabitMQLogConsumer = new DefaultMQLogConsumer(Status.Info, consoleLogger, rabitMqHost, rabitUserName, rabitPassowrd);
            rabitMQLogConsumer = new InformationDecorator(rabitMQLogConsumer);

            rabitMQLogConsumer.Start();

            Console.ReadLine();
        }
    }
}
