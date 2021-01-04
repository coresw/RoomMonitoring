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
        private readonly IMSGraphProvider _graphProvider;
        private readonly IPersonRepository _personRepo;
        private readonly IReservationRepository _reservRepo;
        private readonly IAttendeeRepository _attendRepo;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ReservationSyncService(IMSGraphProvider graphProvider, IPersonRepository personRepo, IReservationRepository reservRepo, IAttendeeRepository attendRepo)
        {
            _graphProvider = graphProvider;
            _personRepo = personRepo;
            _reservRepo = reservRepo;
            _attendRepo = attendRepo;
        }

        public async Task SyncReservations()
        {
            try
            {
                _logger.Info("GetNewReservation started...");

                int reservations = 0, persons = 0, attendees = 0;

                var oReservations = await _graphProvider.GetReservationsAsync();
                var dbReservations = await _reservRepo.GetAll();

                //Pro vsechny mistnosti (dane v DB ciselniku)
                //nacti z O365 pres MS Graph vsechny aktualni schuzky pro dany den. Zajimaji nas pouze ty, ktere jsou od ted do konce dne
                //je treba uvazovat, ze schuzky se prubezne rusi, posunuji a zakladaji
                //schuzky maji sva unikatni ID v O365
                //na zaklade unikatniho ID najit schuzku v DB a provest jeji update ci zalozeni
                //naopak schuzky, ktere jsou v DB a v kolekci  s O365 jiz nejsou, je treba zrusit
                //vysledkem je, ze pri kazdem behu teto metody bude v DB obraz toho co je v O365

                // delete reservations from DB if not in office
                foreach (Reservation dbr in dbReservations)
                {
                    if (oReservations.ToList().Find((OReservation or) => or.Id == dbr.Token) == null) { // reservation from db is not in office
                        await _reservRepo.Delete(dbr);
                        reservations++;
                    }
                }

                // check all reservations for today; add if not in DB or update if changed
                foreach (OReservation or in oReservations)
                {

                    // TODO: office query condition
                    if (or.TimeTo < DateTime.Now || or.TimeFrom >= DateTime.Today.AddDays(1))
                        continue; // skip if already ended or starting next day or later

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
                        await _reservRepo.Update(dbr);
                    }
                    else // reservation is unchanged
                    {
                        continue;
                    }

                    reservations++;

                    // update attendees

                    // delete attendees from DB if not in office
                    foreach (Attendee a in await _attendRepo.GetAll())
                    {
                        if (a.ReservationId != dbr.Id) continue; // not an attendee of this event
                        Person p = await _personRepo.GetById(a.PersonId);
                        if (or.Attendees.ToList().Find((OUser u) => u.EmailAddress == p.EMail) == null
                            && or.Organizer.EmailAddress != p.EMail) // person is not attendee and is not organizer
                        {
                            await _attendRepo.Delete(a);
                            attendees++;
                        }
                    }

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
                            }) ;
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

                Console.WriteLine("Changed: "+reservations+" reservations, "+persons+" persons and "+attendees+" attendees");

                _logger.Info("SyncReservations done!");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Demystify(), "Failed syncing reservations: "+ex.Message);
                throw;
            }
        }

    }
}
