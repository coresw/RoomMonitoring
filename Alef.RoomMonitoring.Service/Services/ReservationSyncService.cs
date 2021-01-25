using Alef.RoomMonitoring.DAL.Model;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Alef.RoomMonitoring.DAL.Services.Interfaces;
using Alef.RoomMonitoring.DAL.Services.Model;
using Alef.RoomMonitoring.Service.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.Service.Services
{
    public class ReservationSyncService : IReservationSyncService
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IMSGraphProvider _graphProvider;
        private readonly IPersonRepository _personRepo;
        private readonly IReservationRepository _reservRepo;
        private readonly IAttendeeRepository _attendRepo;
        private readonly IRoomRepository _roomRepo;

        public ReservationSyncService(IMSGraphProvider graphProvider, IPersonRepository personRepo, IReservationRepository reservRepo, IAttendeeRepository attendRepo, IRoomRepository roomRepo)
        {
            _graphProvider = graphProvider;
            _personRepo = personRepo;
            _reservRepo = reservRepo;
            _attendRepo = attendRepo;
            _roomRepo = roomRepo;
        }

        public async Task SyncReservations()
        {

            try
            {

                _logger.Info("Syncing reservations...");
                int reservations = 0, persons = 0, attendees = 0;

                IEnumerable<Room> rooms = await _roomRepo.GetAll();

                foreach (Room room in rooms)
                {

                    var oReservations = await _graphProvider.GetUpcomingRoomReservations(room.EMail);
                    var dbReservations = await _reservRepo.GetAll();

                    // remove reservations for this room from db if removed not in office
                    foreach (Reservation res in dbReservations) {

                        // keep reservation if not for this room
                        if (res.RoomId != room.Id) continue;

                        // find this reservation in office
                        bool inOffice = oReservations.Any(r =>
                        {
                            return r.Id == res.Token;
                        });

                        if (!inOffice) {
                            await _reservRepo.Delete(res);
                        }

                    }

                    // check all reservations for today; add if not in DB or update if changed
                    foreach (OReservation or in oReservations)
                    {

                        Reservation dbr = await _reservRepo.GetByToken(or.Id);

                        if (dbr == null) // reservation is not in DB
                        {
                            dbr = new Reservation
                            {
                                Token = or.Id,
                                Name = or.Name,
                                Body = or.Body,
                                Created = or.Created,
                                TimeFrom = or.TimeFrom,
                                TimeTo = or.TimeTo,
                                Modified = or.Modified,
                                RoomId = room.Id,
                                ReservationStatusId = ReservationStatus.UNCHECKED.Id,
                            };
                            await _reservRepo.Create(dbr);
                        }
                        else if (or.Modified != dbr.Modified) // reservation was updated
                        {
                            dbr.Name = or.Name;
                            dbr.Body = or.Body;
                            dbr.Modified = or.Modified;
                            dbr.TimeFrom = or.TimeFrom;
                            dbr.TimeTo = or.TimeTo;
                            dbr.RoomId = room.Id;
                            dbr.ReservationStatusId = ReservationStatus.UNCHECKED.Id;
                            await _reservRepo.Update(dbr);
                        }
                        else // reservation is unchanged
                        {
                            continue;
                        }

                        reservations++;

                        // update attendees

                        // create new attendees if not in DB
                        OUser ou = or.Organizer;
                        Person op = await _personRepo.GetByEMail(ou.EmailAddress);
                        if (op == null)
                        { // person is not in DB
                            op = new Person
                            {
                                EMail = ou.EmailAddress,
                                Name = ou.Name,
                            };
                            await _personRepo.Create(op);
                            await _attendRepo.Create(new Attendee
                            {
                                PersonId = op.Id,
                                ReservationId = dbr.Id,
                                AttendeeTypeId = AttendeeType.ORGANIZER.Id,
                            });
                            persons++;
                            attendees++;
                        }
                        else
                        {
                            Attendee a = await _attendRepo.GetByPersonReservation(op.Id, dbr.Id);
                            if (a == null) // attendee is not in DB
                            {
                                await _attendRepo.Create(new Attendee
                                {
                                    PersonId = op.Id,
                                    ReservationId = dbr.Id,
                                    AttendeeTypeId = AttendeeType.ORGANIZER.Id,
                                });
                                attendees++;
                            }
                        }

                        foreach (OUser u in or.Attendees)
                        {
                            Person p = await _personRepo.GetByEMail(u.EmailAddress);
                            if (p == null) // person is not in DB
                            {
                                p = new Person
                                {
                                    EMail = u.EmailAddress,
                                    Name = u.Name,
                                };
                                await _personRepo.Create(p);
                                await _attendRepo.Create(new Attendee
                                {
                                    PersonId = p.Id,
                                    ReservationId = dbr.Id,
                                    AttendeeTypeId = AttendeeType.REQUIRED.Id,
                                });
                                persons++;
                                attendees++;
                            }
                            else
                            {
                                Attendee a = await _attendRepo.GetByPersonReservation(p.Id, dbr.Id);
                                if (a == null) // attendee is not in DB
                                {
                                    await _attendRepo.Create(new Attendee
                                    {
                                        PersonId = p.Id,
                                        ReservationId = dbr.Id,
                                        AttendeeTypeId = AttendeeType.REQUIRED.Id,
                                    });
                                    attendees++;
                                }
                            }
                        }

                    }

                }

                _logger.Info("SyncReservations done! Updated: " + reservations + " reservations, " + persons + " persons and " + attendees + " attendees");

            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed syncing reservations");
            }

        }

    }
}
