using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Logger.Core;

namespace Logger.Implementation.RabitMQ {
    public class DefaultMQLogConsumer : IRabitMQLogConsumer {
        private readonly Status status;
        private readonly ILogger logger;
        private readonly string host;
        private readonly string userName;
        private readonly string password;

        public DefaultMQLogConsumer(Status status, ILogger logger, string host, string userName, string password) {
            this.status = status;
            this.logger = logger;
            this.host = host;
            this.userName = userName;
            this.password = password;
        }

        public void Start() {
            var factory = new ConnectionFactory() { HostName = host, UserName = userName, Password = password, AutomaticRecoveryEnabled = true };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel()) {
                channel.QueueDeclare(queue: status.ToString(),
                                                 durable: false,
                                                 exclusive: false,
                                                 autoDelete: false,
                                                 arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) => {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    logger.Log(status, message);
                };
                channel.BasicConsume(queue: status.ToString(),
                                     autoAck: true,
                                     consumer: consumer);

                while(true) { }
            }
        }
    }
}
