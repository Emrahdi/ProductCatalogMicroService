using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Logger.Core {

    public class ConditionalLogger : ILogger {

        private readonly IDictionary<Status, ILogger> conditionalDictionary;
        private readonly ILogger elseLogger;

        public ConditionalLogger(IDictionary<Status, ILogger> conditionalDictionary, ILogger elseLogger) {
            this.conditionalDictionary = conditionalDictionary;
            this.elseLogger = elseLogger;
        }

        public void Log(Status status, string message, string source = null) {
            if(!conditionalDictionary.ContainsKey(status)) {
                elseLogger.Log(status, message, source);

                return;
            }

            var logger = conditionalDictionary[status];
            logger.Log(status, message, source);
        }
    }
}
