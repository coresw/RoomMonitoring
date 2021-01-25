using Alef.RoomMonitoring.Configuration;
using Alef.RoomMonitoring.Configuration.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Web;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Alef.RoomMonitoring.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            LoggingConfiguration conf = new LoggingConfiguration();
            conf.AddRule(LogLevel.Info, LogLevel.Fatal, new ConsoleTarget());
            LogManager.Configuration = conf;
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .UseNLog()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();
                    services.AddHostedService<RoomMonitoringWorker>();
                    services.AddSingleton<IConfigFileBootstrapLoader, ConfigFileBootstrapLoader>();
                });
    }
}
