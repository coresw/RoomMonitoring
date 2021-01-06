using Alef.RoomMonitoring.Configuration;
using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.Configuration.Model;
using Alef.RoomMonitoring.DAL.Database;
using Alef.RoomMonitoring.DAL.Database.Interfaces;
using Alef.RoomMonitoring.DAL.Repository;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Alef.RoomMonitoring.DAL.Services;
using Alef.RoomMonitoring.DAL.Services.Interfaces;
using Alef.RoomMonitoring.Service.ScheduledJobs;
using Alef.RoomMonitoring.Service.ScheduledJobs.Interfaces;
using Alef.RoomMonitoring.Service.Services;
using Alef.RoomMonitoring.Service.Services.Interfaces;
using CiscoEndpointProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.Service
{
    public class RoomMonitoringWorker : BackgroundService
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private IScheduler _scheduler;
        private readonly IConfigFileBootstrapLoader _configLoader;
        private const string DEFAULT_GROUP = "DefaultGroup";

        public RoomMonitoringWorker(IConfigFileBootstrapLoader configLoader)
        {
            _configLoader = configLoader;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _logger.Info("RoomMonitoringWorker running at: {time}", DateTimeOffset.Now);

            ScheduleJobs();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            _logger.Info("RoomMonitoring service stopped");

        }

        private async void ScheduleJobs()
        {

            _logger.Info("Scheduling jobs...");

            try
            {
                var serviceProvider = GetConfiguredServiceProvider();
                var jobSettings = _configLoader.GetJobSettings();

                ISchedulerFactory schedFact = new StdSchedulerFactory();
                _scheduler = await schedFact.GetScheduler();
                _scheduler.JobFactory = new MonitoringJobFactory(serviceProvider);
                await _scheduler.Start();

                await ScheduleJob<IReservationSyncJob>(jobSettings, ReservationSyncJob.JobName);
                await ScheduleJob<IRoomStatusSyncJob>(jobSettings, RoomStatusSyncJob.JobName);
                await ScheduleJob<ICheckReservationJob>(jobSettings, CheckReservationJob.JobName);

                _logger.Info("ScheduleJobs done!");

            }
            catch (Exception ex)
            {
                _logger.Error("Failed scheduling jobs: "+ex);
                throw;
            }
        }

        private async Task ScheduleJob<T>(IList<JobSetting> settings, string name) where T: IJob
        {

            JobSetting js = settings.FirstOrDefault(x => x.Name == name);

            if (js == null)
                throw new Exception("No config found for "+name+"!");

            if (js.Enabled)
            {
                
                IJobDetail job = JobBuilder.Create<T>()
                                .WithIdentity(name)
                                .Build();

                await _scheduler.ScheduleJob(job, GetTrigger(js));

            }
            else
            {
                _logger.Warn(name+" is disabled - Will not be scheduled!");
            }
        }

        private ITrigger GetTrigger(JobSetting setting)
        {
            ITrigger trigger;
            if (setting.SimpleTriggerTime != null)
            {
                trigger = CreateSimpleTrigger(setting.SimpleTriggerTime.Value, setting.Name);
            }
            else if (setting.TriggerTimeValue != null && setting.TriggerTimeValue.Hour.HasValue && setting.TriggerTimeValue.Minute.HasValue)
            {
                trigger = CreateOnTimeTrigger(setting.TriggerTimeValue.Hour.Value, setting.TriggerTimeValue.Minute.Value, setting.Name);
            }
            else
                throw new Exception("Invalid job configuration!");

            return (trigger);
        }

        private ITrigger CreateSimpleTrigger(int period, string name, string group = DEFAULT_GROUP)
        {
            ITrigger trigger = TriggerBuilder.Create()
           .WithIdentity(name, group)
           .StartNow()
           .WithSimpleSchedule(x => x
               .WithIntervalInSeconds(period)
               .RepeatForever())
           .Build();

            return trigger;
        }

        private ITrigger CreateOnTimeTrigger(int hour, int minute, string name, string group = DEFAULT_GROUP)
        {
            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity(name, group)
              .WithCalendarIntervalSchedule()
              .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hour, minute))
              .Build();
            return trigger;
        }

        private IServiceProvider GetConfiguredServiceProvider()
        {
            var services = new ServiceCollection()
                .AddSingleton<IConfigFileBootstrapLoader, ConfigFileBootstrapLoader>()

                .AddSingleton<IMSGraphProvider, MSGraphProvider>()
                .AddSingleton<IEndpointProvider, MockEndpointProvider>()
                .AddSingleton<IDBProvider, DBProvider>()
                .AddSingleton<IConnectionStringProvider, ConnectionStringProvider>()
                .AddSingleton<IMSGraphAPI, MSGraphAPI>()

                .AddSingleton<IReservationRepository, ReservationRepository>()
                .AddSingleton<IRoomRepository, RoomRepository>()
                .AddSingleton<IPersonRepository, PersonRepository>()
                .AddSingleton<IAttendeeRepository, AttendeeRepository>()

                .AddSingleton<ICheckReservationService, CheckReservationService>()
                .AddSingleton<IReservationSyncService, ReservationSyncService>()
                .AddSingleton<IRoomStatusSyncService, RoomStatusSyncService>()

                .AddSingleton<ICheckReservationJob, CheckReservationJob>()
                .AddSingleton<IReservationSyncJob, ReservationSyncJob>()
                .AddSingleton<IRoomStatusSyncJob, RoomStatusSyncJob>()

                .AddLogging();

            return services.BuildServiceProvider();
        }
    }
}
