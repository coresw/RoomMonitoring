using Alef.RoomMonitoring.Configuration.ConfigFileSections;
using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.DAL.Database.WhereConstraints;
using Alef.RoomMonitoring.DAL.Model;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Alef.RoomMonitoring.Service.Services.Interfaces;
using CiscoEndpointProvider;
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
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IReservationRepository _reservRepo;
        private readonly IRoomRepository _roomRepo;
        private readonly IEndpointProvider _endpoint;
        private readonly IConfigFileBootstrapLoader _config;
        
        public CheckReservationService(IReservationRepository reservRepo, IRoomRepository roomRepo, IEndpointProvider endpoint, IConfigFileBootstrapLoader config)
        {
            _reservRepo = reservRepo;
            _roomRepo = roomRepo;
            _endpoint = endpoint;
            _config = config;
        }

        public void CheckReservations()
        {

            try
            {

                _logger.Info("Checking reservations...");

                IEnumerable<Reservation> reservations = _reservRepo.GetWhere(
                    new AndConstraint(
                        new NotEqualConstraint(Reservation.RESERVATION_STATUS_ID, ReservationStatus.CLOSED.Id)
                        )
                    );

                var ActiveTimeout = _config.GetReservationSettings().CheckTimeout;

                foreach (Reservation r in reservations) {

                    if (DateTime.Now.ToUniversalTime() < r.TimeFrom) continue;

                    if (DateTime.Now.ToUniversalTime() < r.TimeFrom.AddMinutes(ActiveTimeout)) {
                        if (r.ReservationStatusId == ReservationStatus.PLANNED.Id)
                        {
                            r.ReservationStatusId = ReservationStatus.ACTIVE.Id;
                            _reservRepo.Update(r);
                        }
                        continue;
                    }

                    if (DateTime.Now.ToUniversalTime() > r.TimeTo) {
                        r.ReservationStatusId = ReservationStatus.CLOSED.Id;
                        _reservRepo.Update(r);
                        continue;
                    }

                    Room room = _roomRepo.GetById(r.RoomId);
                    bool occupied = _endpoint.GetPeopleCount(room.EndpointIP) > 0;

                    if (occupied)
                    {
                        if (r.ReservationStatusId != ReservationStatus.ACTIVE.Id) {
                            r.ReservationStatusId = ReservationStatus.ACTIVE.Id;
                            _reservRepo.Update(r);
                            _logger.Info("Room " + room.Name + " status changed to active");
                        }
                    }
                    else
                    {
                        if (r.ReservationStatusId != ReservationStatus.EMPTY.Id) {

                            // TODO: implement notification logic

                            r.ReservationStatusId = ReservationStatus.EMPTY.Id;
                            _reservRepo.Update(r);
                        }
                        _logger.Info("Room "+room.Name+" status changed to empty! Attendees will be notified");
                    }

                }

                _logger.Info("Checking reservations done!");

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Demystify(), "Failed checking reservations");
                throw;
            }
        }

    }
}
