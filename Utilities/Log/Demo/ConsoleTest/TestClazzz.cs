using System;
using System.Collections.Generic;
using System.Text;
using Logger.Core;

namespace ConsoleTest {
    public class TestClazzz {

        public static void Log(ILogger logger, string message) {
            logger.Log(Status.Error, message);
        }
    }
}
