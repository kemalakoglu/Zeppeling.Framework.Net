using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Zeppeling.Infrastructure.Core.Response
{
    public static class GetResponseCode
    {
        public static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
        }
        public static string GetDescription(string RC)
        {
            IConfiguration config = GetConfiguration();
            return config.GetSection("ResponseCodes:"+RC).Get<string>();
        }
    }
}
