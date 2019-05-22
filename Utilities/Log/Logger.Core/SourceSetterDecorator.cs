using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Logger.Core {
    public class SourceSetterDecorator : ILogger {
        private readonly ILogger decorated;

        public SourceSetterDecorator(ILogger decorated) {
            this.decorated = decorated;
        }

        public void Log(Status status, string message, [CallerMemberName] string source = null) {
            decorated.Log(status, message, source);
        }
    }
}
