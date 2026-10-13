using Autofac.Extensions.DependencyInjection;
using Serilog;

namespace ClinicManagement.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    webBuilder.UseStartup<Startup>().UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));
#pragma warning restore CS0618 // Type or member is obsolete
                });
    }
}