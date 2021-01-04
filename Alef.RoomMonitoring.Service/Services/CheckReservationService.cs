using Alef.RoomMonitoring.Configuration.ConfigFileSections;
using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.DAL.Model;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Alef.RoomMonitoring.Service.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.Service.Services
{
    /// <summary>
    /// Trida implementujici funkcnost kontroly 
    /// </summary>
    public class CheckReservationService : ICheckReservationService
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IReservationRepository _reservRepo;
        private readonly IRoomRepository _roomRepo;
        private readonly DbConfiguration _dbConf;

        #region Ctor

        public CheckReservationService(IReservationRepository reservRepo, IRoomRepository roomRepo, IConfigFileBootstrapLoader config)
        {
            _reservRepo = reservRepo;
            _roomRepo = roomRepo;
            _dbConf = config.GetDbConfiguration();
        }

        #endregion

        #region Public methods

        public async Task CheckReservations()
        {
            try
            {

                //Nacist rezervace z DB. Nacitaji se pouze rezervace, ktere nebyly jiz zpracovany tj. notifikovany atd.

                IEnumerable<Reservation> reservations = await _reservRepo.GetWhere(
                    "ReservationStatusId="+ReservationStatus.UNCHECKED.Id+
                    " and TimeFrom<'"+DateTime.Now.ToString(_dbConf.DateFormat)+"'" +
                    " and TimeTo<'"+DateTime.Today.AddDays(1).ToString(_dbConf.DateFormat)
                    );

                foreach (Reservation r in reservations) {

                    bool occupied = _roomRepo.GetById(r.RoomId).Result.Occupied;

                    if (occupied)
                    {
                        r.ReservationStatusId = ReservationStatus.OK.Id;
                    }
                    else
                    {
                        // TODO: implement notification logic
                        r.ReservationStatusId = ReservationStatus.NOTIFIED.Id;
                    }

                    await _reservRepo.Update(r);

                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Demystify(), "Failed checking reservations: "+ex);
                throw;
            }
        }

        #endregion
    }
}
