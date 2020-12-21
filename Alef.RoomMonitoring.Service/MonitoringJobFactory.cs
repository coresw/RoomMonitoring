using Quartz;
using Quartz.Spi;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Alef.RoomMonitoring.Service
{
    public class MonitoringJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProdvider;
        public MonitoringJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProdvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return _serviceProdvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job) => (job as IDisposable)?.Dispose();
    }
}
