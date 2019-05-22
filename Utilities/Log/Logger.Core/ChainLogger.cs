using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Logger.Core {
    public class ChainLogger : ILogger {
        private readonly ILogger first;
        private readonly ILogger second;

        public ChainLogger(ILogger first, ILogger second) {
            this.first = first;
            this.second = second;
        }

        public void Log(Status status, string message, string source = null) {
            try {
                first.Log(status, message, source);
            }
            catch {
                second.Log(status, message);
            }
        }
    }
}
