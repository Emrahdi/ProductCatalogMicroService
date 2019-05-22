using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using RabbitMQ.Client;
using Logger.Core;

namespace Logger.Implementation.RabitMQ {
    public class RabbitMQLogger : ILogger {
        private readonly string host;
        private readonly string userName;
        private readonly string password;

        public RabbitMQLogger(string host, string userName, string password) {
            this.host = host;
            this.userName = userName;
            this.password = password;
        }

        public void Log(Status status, string message, string source = null) {
            var factory = new ConnectionFactory() { HostName = host, UserName = userName, Password = password };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: status.ToString(),
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: status.ToString(),
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
