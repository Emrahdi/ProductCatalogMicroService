using System;
using System.Collections.Generic;
using System.Text;

namespace Logger.Implementation.RabitMQ {
    public class InformationDecorator : IRabitMQLogConsumer {

        private readonly IRabitMQLogConsumer decorated;

        public InformationDecorator(IRabitMQLogConsumer decorated) {
            this.decorated = decorated;
        }
        public void Start() {
            decorated.Start();
            Console.WriteLine("Consumer started");
        }
    }
}
