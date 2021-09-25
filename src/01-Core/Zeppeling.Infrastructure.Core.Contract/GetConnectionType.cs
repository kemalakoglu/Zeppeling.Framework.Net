using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Zeppeling.Infrastructure.Core.Contract
{
    public static class GetConnectionType
    {

        public static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
        }

        public static ConnectionType Get(string contextName)
        {
            IConfiguration config = GetConfiguration();
            return config.GetSection(contextName+":0").Get<ConnectionType>();
        }
    }
}
