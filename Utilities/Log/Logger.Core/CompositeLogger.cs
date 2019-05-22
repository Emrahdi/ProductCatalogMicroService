using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Logger.Core {

    public class CompositeLogger : ILogger {
        private readonly ILogger[] loggers;

        public CompositeLogger(params ILogger[] loggers) {
            this.loggers = loggers;
        }

        public void Log(Status status, string message, string source = null) {
            foreach (var item in loggers) {
                item.Log(status, message, source);
            }
        }
    }
}
