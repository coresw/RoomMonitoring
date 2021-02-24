using Alef.RoomMonitoring.DAL.Model;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Alef.RoomMonitoring.Service.Services.Interfaces;
using CiscoEndpointProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Alef.RoomMonitoring.Test
{
    [TestClass]
    public class RoomMonitoringTest: TestBase
    {

        private IPersonRepository _personRepo;
        private IReservationRepository _reservRepo;
        private IRoomRepository _roomRepo;
        private IAttendeeRepository _attendeeRepo;
        private IAttendeeTypeRepository _attendeeTypeRepo;
        private IReservationStatusRepository _reservStatusRepo;
        private IMockEndpointRepository _mockEndpointRepo;
        
        private ICheckReservationService _checkService;

        public RoomMonitoringTest() {

            _personRepo = serviceProvider.GetService<IPersonRepository>();
            _reservRepo = serviceProvider.GetService<IReservationRepository>();
            _roomRepo = serviceProvider.GetService<IRoomRepository>();
            _attendeeRepo = serviceProvider.GetService<IAttendeeRepository>();
            _attendeeTypeRepo = serviceProvider.GetService<IAttendeeTypeRepository>();
            _reservStatusRepo = serviceProvider.GetService<IReservationStatusRepository>();
            _mockEndpointRepo = serviceProvider.GetService<IMockEndpointRepository>();

            _checkService = serviceProvider.GetService<ICheckReservationService>();

        }

        [TestMethod]
        public void TestUpcoming() {

            Person p = _personRepo.GetByProperty(Person.EMAIL, "johndoe@email.com");

            if (p == null) {
                p = new Person()
                {
                    Name = "John Doe",
                    EMail = "johndoe@email.com"
                };
                _personRepo.Create(p);
            }

            Room r = _roomRepo.GetByProperty(Room.NAME, "MeetingRoom");

            if (r == null) {
                r = new Room()
                {
                    Name = "MeetingRoom",
                    EMail = "meetingroom@company.com",
                    EndpointIP = "M33TR00M"
                };
                _roomRepo.Create(r);
            }

            Reservation res = new Reservation()
            {
                Token = DateTime.Now.Ticks.ToString(),
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Name = "Meeting",
                Body = "A Testing Meeting",
                TimeFrom = DateTime.Now.ToUniversalTime().AddHours(1),
                TimeTo = DateTime.Now.ToUniversalTime().AddHours(2),
                RoomId = r.Id,
            };

            _reservRepo.Create(res);

            Attendee att = new Attendee() {
                PersonId = p.Id,
                ReservationId = res.Id,
                AttendeeTypeId = _attendeeTypeRepo.GetByProperty(AttendeeType.NAME, AttendeeType.ORGANIZER).Id,
            };

            _attendeeRepo.Create(att);

            _checkService.CheckReservations();

            var status = _reservStatusRepo.GetByProperty(ReservationStatus.ID,
                _reservRepo.GetByProperty(Reservation.ID, res.Id).ReservationStatusId).Name;

            Debug.Write(status);

            Assert.AreEqual(status, ReservationStatus.PLANNED);
            
        }

        [TestMethod]
        public void TestBeforeTimeout()
        {

            Person p = _personRepo.GetByProperty(Person.EMAIL, "johndoe@email.com");

            if (p == null)
            {
                p = new Person()
                {
                    Name = "John Doe",
                    EMail = "johndoe@email.com"
                };
                _personRepo.Create(p);
            }

            Room r = _roomRepo.GetByProperty(Room.NAME, "MeetingRoom");

            if (r == null)
            {
                r = new Room()
                {
                    Name = "MeetingRoom",
                    EMail = "meetingroom@company.com",
                    EndpointIP = "M33TR00M"
                };
                _roomRepo.Create(r);
            }

            Reservation res = new Reservation()
            {
                Token = DateTime.Now.Ticks.ToString(),
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Name = "Meeting",
                Body = "A Testing Meeting",
                TimeFrom = DateTime.Now.ToUniversalTime().AddMinutes(-5),
                TimeTo = DateTime.Now.ToUniversalTime().AddMinutes(55),
                RoomId = r.Id,
            };

            _reservRepo.Create(res);

            Attendee att = new Attendee()
            {
                PersonId = p.Id,
                ReservationId = res.Id,
                AttendeeTypeId = _attendeeTypeRepo.GetByProperty(AttendeeType.NAME, AttendeeType.ORGANIZER).Id,
            };

            _attendeeRepo.Create(att);

            _checkService.CheckReservations();

            var status = _reservStatusRepo.GetByProperty(ReservationStatus.ID,
                _reservRepo.GetByProperty(Reservation.ID, res.Id).ReservationStatusId).Name;

            Debug.Write(status);

            Assert.AreEqual(status, ReservationStatus.ACTIVE);

        }

        [TestMethod]
        public void TestEmpty()
        {

            MockEndpoint ep = _mockEndpointRepo.GetByProperty(MockEndpoint.ENDPOINT_IP, "M33TR00M");
            if (ep == null) {
                ep = new MockEndpoint()
                {
                    EndpointIp = "M33TR00M",
                };
                _mockEndpointRepo.Create(ep);
            }

            ep.PeopleCount = 0;
            _mockEndpointRepo.Update(ep);

            Person p = _personRepo.GetByProperty(Person.EMAIL, "johndoe@email.com");

            if (p == null)
            {
                p = new Person()
                {
                    Name = "John Doe",
                    EMail = "johndoe@email.com"
                };
                _personRepo.Create(p);
            }

            Room r = _roomRepo.GetByProperty(Room.NAME, "MeetingRoom");

            if (r == null)
            {
                r = new Room()
                {
                    Name = "MeetingRoom",
                    EMail = "meetingroom@company.com",
                    EndpointIP = "M33TR00M"
                };
                _roomRepo.Create(r);
            }

            Reservation res = new Reservation()
            {
                Token = DateTime.Now.Ticks.ToString(),
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Name = "Meeting",
                Body = "A Testing Meeting",
                TimeFrom = DateTime.Now.ToUniversalTime().AddMinutes(-30),
                TimeTo = DateTime.Now.ToUniversalTime().AddMinutes(30),
                RoomId = r.Id,
            };

            _reservRepo.Create(res);

            Attendee att = new Attendee()
            {
                PersonId = p.Id,
                ReservationId = res.Id,
                AttendeeTypeId = _attendeeTypeRepo.GetByProperty(AttendeeType.NAME, AttendeeType.ORGANIZER).Id,
            };

            _attendeeRepo.Create(att);

            _checkService.CheckReservations();

            var status = _reservStatusRepo.GetByProperty(ReservationStatus.ID,
                _reservRepo.GetByProperty(Reservation.ID, res.Id).ReservationStatusId).Name;

            Debug.Write(status);

            Assert.AreEqual(status, ReservationStatus.EMPTY);

        }

        [TestMethod]
        public void TestActive()
        {

            MockEndpoint ep = _mockEndpointRepo.GetByProperty(MockEndpoint.ENDPOINT_IP, "M33TR00M");
            if (ep == null)
            {
                ep = new MockEndpoint()
                {
                    EndpointIp = "M33TR00M",
                };
                _mockEndpointRepo.Create(ep);
            }

            ep.PeopleCount = 1;
            _mockEndpointRepo.Update(ep);

            Person p = _personRepo.GetByProperty(Person.EMAIL, "johndoe@email.com");

            if (p == null)
            {
                p = new Person()
                {
                    Name = "John Doe",
                    EMail = "johndoe@email.com"
                };
                _personRepo.Create(p);
            }

            Room r = _roomRepo.GetByProperty(Room.NAME, "MeetingRoom");

            if (r == null)
            {
                r = new Room()
                {
                    Name = "MeetingRoom",
                    EMail = "meetingroom@company.com",
                    EndpointIP = "M33TR00M"
                };
                _roomRepo.Create(r);
            }

            Reservation res = new Reservation()
            {
                Token = DateTime.Now.Ticks.ToString(),
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Name = "Meeting",
                Body = "A Testing Meeting",
                TimeFrom = DateTime.Now.ToUniversalTime().AddMinutes(-30),
                TimeTo = DateTime.Now.ToUniversalTime().AddMinutes(30),
                RoomId = r.Id,
            };

            _reservRepo.Create(res);

            Attendee att = new Attendee()
            {
                PersonId = p.Id,
                ReservationId = res.Id,
                AttendeeTypeId = _attendeeTypeRepo.GetByProperty(AttendeeType.NAME, AttendeeType.ORGANIZER).Id,
            };

            _attendeeRepo.Create(att);

            _checkService.CheckReservations();

            var status = _reservStatusRepo.GetByProperty(ReservationStatus.ID,
                _reservRepo.GetByProperty(Reservation.ID, res.Id).ReservationStatusId).Name;

            Debug.Write(status);

            Assert.AreEqual(status, ReservationStatus.ACTIVE);

        }

    }
}
