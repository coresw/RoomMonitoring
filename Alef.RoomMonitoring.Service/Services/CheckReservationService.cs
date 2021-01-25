using Alef.RoomMonitoring.Configuration.ConfigFileSections;
using Alef.RoomMonitoring.Configuration.Interfaces;
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

        public async Task CheckReservations()
        {

            try
            {

                _logger.Info("Checking reservations...");

                IEnumerable<Reservation> reservations = await _reservRepo.GetWhere(
                    "ReservationStatusId='"+ReservationStatus.UNCHECKED.Id+"'"+
                    " and TimeFrom>='"+DateTime.Now.ToUniversalTime().ToString(_config.GetDbConfiguration().DateFormat)+"'"+
                    " and TimeTo<='"+DateTime.Today.AddDays(1).ToUniversalTime().ToString(_config.GetDbConfiguration().DateFormat) +"'"
                    );

                foreach (Reservation r in reservations) {

                    Room room = await _roomRepo.GetById(r.RoomId);
                    bool occupied = _endpoint.GetPeopleCount(room.EndpointIP)>0;

                    if (occupied)
                    {
                        r.ReservationStatusId = ReservationStatus.OK.Id;
                        _logger.Info("Room " + room.Name + " status: OK");
                    }
                    else
                    {
                        // TODO: implement notification logic
                        r.ReservationStatusId = ReservationStatus.NOTIFIED.Id;
                        _logger.Info("Room "+room.Name+" has a reservation but is not occupied!");
                    }

                    await _reservRepo.Update(r);

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
