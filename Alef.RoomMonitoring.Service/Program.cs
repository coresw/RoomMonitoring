using Alef.RoomMonitoring.Configuration;
using Alef.RoomMonitoring.Configuration.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .UseNLog()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<RoomMonitoringWorker>();
                    services.AddSingleton<IConfigFileBootstrapLoader, ConfigFileBootstrapLoader>();
                    services.AddSingleton<IJobFactory, MonitoringJobFactory>();
                    services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
                });
    }
}
