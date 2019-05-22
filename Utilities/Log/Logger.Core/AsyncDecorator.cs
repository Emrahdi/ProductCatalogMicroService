using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Logger.Core {
    public class AsyncDecorator : ILogger {
        private readonly ILogger decorated;

        public AsyncDecorator(ILogger decorated) {
            this.decorated = decorated;
        }

        public void Log(Status status, string message, string source = null) {
            Task.Run(() => decorated.Log(status, message, source));
        }
    }
}
