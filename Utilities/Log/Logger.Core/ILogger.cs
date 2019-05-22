using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Logger.Core {
    public interface ILogger {

        void Log(Status status, string message, [CallerMemberName] string sourceMethod = null);
    }
}
