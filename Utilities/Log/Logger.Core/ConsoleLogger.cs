using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Logger.Core {
    public class ConsoleLogger : ILogger {
        public void Log(Status status, string message, string source = null) {
            Console.WriteLine($"Console log : {status} - {message}");
        }
    }
}
