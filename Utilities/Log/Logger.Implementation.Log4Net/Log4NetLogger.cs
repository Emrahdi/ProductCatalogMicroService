using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Runtime.CompilerServices;
using Logger.Core;
using log4net;

namespace Logger.Implementation.Log4Net {
    public class Log4NetLogger : ILogger {

        protected static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly IDictionary<Status, Action<string>> statusDictionary;

        static Log4NetLogger() {
            statusDictionary = new Dictionary<Status, Action<string>>() {
                { Status.Info, log.Info },
                { Status.Debug, log.Debug },
                { Status.Warn, log.Warn },
                { Status.Error, log.Error },
                { Status.Fatal, log.Fatal}
            };
        }

        public Log4NetLogger(string configName) {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead(configName));
            var repo = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(),
                       typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
        }

        public void Log(Status status, string message, string source = null) {
            statusDictionary[status](message);
        }
    }
}
