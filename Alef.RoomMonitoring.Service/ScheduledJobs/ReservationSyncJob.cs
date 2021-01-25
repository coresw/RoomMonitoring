using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Alef.RoomMonitoring.Service.ScheduledJobs.Interfaces;
using NLog;
using Quartz;
using System;
using System.Threading.Tasks;
using System.Linq;
using Alef.RoomMonitoring.Service.Services;
using Alef.RoomMonitoring.Service.Services.Interfaces;
using System.Diagnostics;

namespace Alef.RoomMonitoring.Service.ScheduledJobs
{
    public class ReservationSyncJob : IReservationSyncJob
    {
        public static string JobName { get => "ReservationSyncJob"; }
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly IReservationSyncService _service;

        public ReservationSyncJob(IReservationSyncService service)
        {
            _service = service;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _service.SyncReservations();
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "ReservationSyncJob failed");
            }
        }

    }
}
