using Logger.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Logger.Implementation.Log4Net {
    public class CustomPropertiesSetterDecorator : ILogger {

        private readonly ILogger decorated;

        public CustomPropertiesSetterDecorator(ILogger decorated) {
            this.decorated = decorated;
        }

        public void Log(Status status, string message, string source = null) {
            log4net.LogicalThreadContext.Properties["Clazz"] = source;
            log4net.LogicalThreadContext.Properties["Method"] = source;

            decorated.Log(status, message);
        }
    }
}
