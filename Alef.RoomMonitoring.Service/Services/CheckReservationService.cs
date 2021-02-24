using Alef.RoomMonitoring.Configuration.ConfigFileSections;
using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.Configuration.Model;
using Alef.RoomMonitoring.DAL.Database.WhereConstraints;
using Alef.RoomMonitoring.DAL.Model;
using Alef.RoomMonitoring.DAL.Repository;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Alef.RoomMonitoring.DAL.Services.Interfaces;
using Alef.RoomMonitoring.Service.Services.Interfaces;
using CiscoEndpointProvider;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private readonly IReservationStatusRepository _reservStatusRepo;
        private readonly IRoomRepository _roomRepo;
        private readonly IAttendeeRepository _attendeeRepo;
        private readonly IAttendeeTypeRepository _attendeeTypeRepo;
        private readonly IPersonRepository _personRepo;
        private readonly IMSGraphProvider _graph;
        private readonly IEndpointProvider _endpoint;
        private readonly IConfigFileBootstrapLoader _config;
        
        public CheckReservationService(IReservationRepository reservRepo, IReservationStatusRepository reservStatusRepo, IRoomRepository roomRepo, IAttendeeRepository attendeeRepo, IAttendeeTypeRepository attendeeTypeRepo, IPersonRepository personRepo, IMSGraphProvider graph, IEndpointProvider endpoint, IConfigFileBootstrapLoader config)
        {
            _reservRepo = reservRepo;
            _reservStatusRepo = reservStatusRepo;
            _roomRepo = roomRepo;
            _attendeeRepo = attendeeRepo;
            _attendeeTypeRepo = attendeeTypeRepo;
            _personRepo = personRepo;
            _graph = graph;
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
                        new NotEqualConstraint(Reservation.RESERVATION_STATUS_ID,
                            _reservStatusRepo.GetByProperty(ReservationStatus.NAME, ReservationStatus.CLOSED).Id)
                        )
                    );

                ReservationSettings settings = _config.GetReservationSettings();

                var ActiveTimeout = settings.CheckTimeout;

                foreach (Reservation r in reservations) {

                    if (DateTime.Now.ToUniversalTime() < r.TimeFrom) continue;

                    if (DateTime.Now.ToUniversalTime() < r.TimeFrom.AddMinutes(ActiveTimeout)) {
                        if (r.ReservationStatusId == _reservStatusRepo.GetByProperty(ReservationStatus.NAME, ReservationStatus.PLANNED).Id)
                        {
                            r.ReservationStatusId = _reservStatusRepo.GetByProperty(ReservationStatus.NAME, ReservationStatus.ACTIVE).Id;
                            _reservRepo.Update(r);
                        }
                        continue;
                    }

                    if (DateTime.Now.ToUniversalTime() > r.TimeTo) {
                        r.ReservationStatusId = _reservStatusRepo.GetByProperty(ReservationStatus.NAME, ReservationStatus.CLOSED).Id;
                        _reservRepo.Update(r);
                        continue;
                    }

                    Room room = _roomRepo.GetByProperty(Room.ID, r.RoomId);
                    bool occupied = _endpoint.GetPeopleCount(room.EndpointIP) > 0;

                    if (occupied)
                    {
                        if (r.ReservationStatusId != _reservStatusRepo.GetByProperty(ReservationStatus.NAME, ReservationStatus.ACTIVE).Id) {
                            r.ReservationStatusId = _reservStatusRepo.GetByProperty(ReservationStatus.NAME, ReservationStatus.ACTIVE).Id;
                            _reservRepo.Update(r);
                            _logger.Info("Room " + room.Name + " status changed to active");
                        }
                    }
                    else
                    {
                        if (r.ReservationStatusId != _reservStatusRepo.GetByProperty(ReservationStatus.NAME, ReservationStatus.EMPTY).Id) {

                            Person p = _personRepo.GetByProperty(Person.ID,
                                _attendeeRepo.GetWhere(new AndConstraint(
                                    new EqualConstraint(Attendee.RESERVATION_ID, r.Id),
                                    new EqualConstraint(Attendee.ATTENDEE_TYPE_ID, 
                                        _attendeeTypeRepo.GetByProperty(AttendeeType.NAME, AttendeeType.ORGANIZER).Id)
                                    )
                                ).First().Id);

                            IEnumerable<Attendee> attendees = _attendeeRepo.GetWhere(new EqualConstraint(Attendee.RESERVATION_ID, r.Id));
                            IEnumerable<string> persons = attendees.Cast<string>();

                            /*string notification = settings.NotificationBody
                                .Replace("%ReservName%", r.Name)
                                .Replace("%ReservBody%", r.Body)
                                .Replace("%ReservFrom%", r.TimeFrom.ToString())
                                .Replace("%ReservTo%", r.TimeFrom.ToString())
                                .Replace("%Organizer%", p.Name)
                                .Replace("%Room%", room.Name)
                                ;

                            _graph.SendMail(room.EMail, settings.NotificationTitle, notification, persons);*/

                            r.ReservationStatusId = _reservStatusRepo.GetByProperty(ReservationStatus.NAME, ReservationStatus.EMPTY).Id;
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
