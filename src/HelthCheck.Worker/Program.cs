using System.IO;
using System.Threading.Tasks;
using HelthCheck.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HelthCheck.Worker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(hostConfig =>
                {
                    hostConfig.SetBasePath(Directory.GetCurrentDirectory());
                    hostConfig.AddJsonFile("hostsettings.json", optional: false);
                    hostConfig.AddEnvironmentVariables(prefix: "HELTH_");
                    hostConfig.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostConfig, appConfig) =>
                {
                    appConfig.AddJsonFile("appsettings.json", optional: true);
                    appConfig.AddJsonFile($"appsettings.{hostConfig.HostingEnvironment.EnvironmentName}.json", optional: false);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<ICronJob, CronJob>();

                    services.AddDbContext<ApplicationContext>(options =>
                        options.UseSqlite(hostContext.Configuration.GetConnectionString("ApplicationContext")));

                    services.AddHostedService<WorkerService>();
                })
                .ConfigureLogging((appContext, loggerBuilder) =>
                {
                    loggerBuilder.AddConsole(o =>
                    {
                        o.TimestampFormat = "[dd-MM-yyyy HH:mm:ss] ";
                    });
                    loggerBuilder.AddDebug();
                })
                .UseConsoleLifetime()
                .Build();

            await host.RunAsync();
        }
    }
}
