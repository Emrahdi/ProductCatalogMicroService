using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUIGateWayApi.Authentication {
    public class ConfigurationKeyProvider : IKeyProvider {
        private readonly IConfiguration configuration;

        const string appsettingsKey= "SecretKey";
        public ConfigurationKeyProvider(IConfiguration configuration) {
            this.configuration = configuration;
        }
        public string GetSecretKey() {
            var key = configuration.GetSection(appsettingsKey).Get<string>();
            return key;
        }
    }
}
