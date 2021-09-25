using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Zeppeling.Infrastructure.Presentation.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                //.UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>();
        }

        //public static IHostBuilder CreateWebHostBuilder(string[] args)
        //{
        //    return Host.CreateDefaultBuilder(args)
        //         //.UseKestrel()
        //         .UseContentRoot(Directory.GetCurrentDirectory())
        //         .ConfigureWebHostDefaults(webBuilder =>
        //         {
        //             webBuilder.UseStartup<Startup>();
        //             webBuilder.UseIISIntegration();
        //         })
        //        //.UseStartup<Startup>()
        //        .UseServiceProviderFactory(new DynamicProxyServiceProviderFactory());

        //}
    }
}