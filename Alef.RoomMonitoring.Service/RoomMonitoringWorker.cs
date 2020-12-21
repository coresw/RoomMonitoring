using Alef.RoomMonitoring.Configuration;
using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.Configuration.Model;
using Alef.RoomMonitoring.DAL.Repository;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Alef.RoomMonitoring.DAL.Services;
using Alef.RoomMonitoring.DAL.Services.Interfaces;
using Alef.RoomMonitoring.Service.ScheduledJobs;
using Alef.RoomMonitoring.Service.ScheduledJobs.Interfaces;
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

            StartJobs();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            _logger.Info("RoomMonitoring service stopped");
        }

        private async void StartJobs()
        {
            try
            {
                var serviceProvider = GetConfiguredServiceProvider();
                var jobSettings = _configLoader.GetJobSettings();

                ISchedulerFactory schedFact = new StdSchedulerFactory();
                _scheduler = await schedFact.GetScheduler();
                _scheduler.JobFactory = new MonitoringJobFactory(serviceProvider);
                await _scheduler.Start();

                await ScheduleGetRoomReservationJob(jobSettings);
                await ScheduleCheckReservationJob(jobSettings);

                _logger.Info("RoomMonitoring service - all jobs started.....");
            }
            catch (Exception ex)
            {
                _logger.Error("Unexpected error");
                throw ex;
            }
        }

        private async Task ScheduleGetRoomReservationJob(IList<JobSetting> settings)
        {
            JobSetting js = settings.FirstOrDefault(x => x.Name == "GetRoomReservationJob");

            if (js == null)
                throw new ArgumentNullException("Config.xml does not containg job configuration for ProductModify job ");

            if (js.Enabled)
            {
                IJobDetail job = JobBuilder.Create<IGetRoomReservationJob>()
                                .WithIdentity(GetRoomReservationJob.JobName)
                                .Build();

                await _scheduler.ScheduleJob(job, GetTrigger(js));

                _logger.Info("ScheduleProductModifyJob scheduled");
            }
            else
            {
                _logger.Info("ScheduleProductModifyJob was not enabled");
            }
        }

        private async Task ScheduleCheckReservationJob(IList<JobSetting> settings)
        { 
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
                throw new ArgumentException("Wrong job configuration");

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
                .AddSingleton<IConnectionStringProvider, ConnectionStringProvider>()
                .AddScoped<IPersonRepository, PersonRepository>()
                .AddScoped<IAttendeeRepository, AttendeeRepository>()
                .AddScoped<IRoomRepository, RoomRepository>()
                .AddScoped<IReservationRepository, ReservationRepository>()
                .AddScoped<IAttendeeTypeRepository, AttendeeTypeRepository>()
                .AddScoped<IMSGraphAPI, MSGraphAPI>()
                .AddScoped<IGetRoomReservationJob, GetRoomReservationJob>()
                .AddScoped<ICheckReservationJob, CheckReservationJob>()
                .AddLogging();

            return services.BuildServiceProvider();
        }
    }
}
