using Alef.RoomMonitoring.Service.ScheduledJobs.Interfaces;
using Alef.RoomMonitoring.Service.Services;
using Alef.RoomMonitoring.Service.Services.Interfaces;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.Service.ScheduledJobs
{
    class RoomStatusSyncJob : IRoomStatusSyncJob
    {

        public static string JobName { get => "RoomStatusSyncJob"; }
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly IRoomStatusSyncService _service;

        public RoomStatusSyncJob(IRoomStatusSyncService service)
        {
            _service = service;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _service.SyncRooms();
            }
            catch (Exception e)
            {
                _logger.Error("RoomStatusSyncJob failed: "+e.Message);
            }
        }
    }
}
