using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Alef.RoomMonitoring.Service.ScheduledJobs.Interfaces;
using NLog;
using Quartz;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Alef.RoomMonitoring.Service.ScheduledJobs
{
    public class GetRoomReservationJob : IGetRoomReservationJob
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private IConfigFileBootstrapLoader _configLoader;
        private IPersonRepository _personRepo;

        #region Ctor
        public GetRoomReservationJob(IConfigFileBootstrapLoader configLoader, IPersonRepository personRepo)
        {
            _configLoader = configLoader;
            _personRepo = personRepo;
        }

        #endregion

        #region Public methods

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.Info($"Do GetRoomReservationJob at time = {DateTimeOffset.Now}");

                var persons = await _personRepo.GetAll();

                if (persons != null && persons.Count() > 0)
                    ;
            }
            catch (Exception ex)
            {
                _logger.Error($"Unexpected error ");
            }
        }

        public static string JobName { get { return ("GetRoomReservationJob"); } }

        #endregion
    }
}
