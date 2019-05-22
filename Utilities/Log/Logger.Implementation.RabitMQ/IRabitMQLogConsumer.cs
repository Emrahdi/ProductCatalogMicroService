using System;
using System.Collections.Generic;
using System.Text;

namespace Logger.Implementation.RabitMQ {
    public interface IRabitMQLogConsumer {

        void Start();
    }
}
