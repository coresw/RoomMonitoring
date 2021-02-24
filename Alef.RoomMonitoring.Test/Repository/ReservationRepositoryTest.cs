using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Alef.RoomMonitoring.DAL.Model;
using System;
using Alef.RoomMonitoring.DAL.Database.WhereConstraints;

namespace Alef.RoomMonitoring.Test.Repository
{
    [TestClass]
    public class ReservationRepositoryTest : TestBase
    {

        private IReservationRepository _repo;

        public ReservationRepositoryTest() {

            _repo = serviceProvider.GetService<IReservationRepository>();

        }

        [TestMethod]
        public void TestCreate() {

            Reservation r = new Reservation()
            {
                Name = "Test Reservation",
                Body = "Reservation Body",
                TimeFrom = new DateTime(2021, 2, 24, 12, 0, 0),
                TimeTo = new DateTime(2021, 2, 24, 13, 0, 0),
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Token = DateTime.Now.Ticks.ToString(),
            };

            _repo.Create(r);

            Debug.WriteLine(r.Id);

            Assert.IsTrue(r.Id>0);

        }

        [TestMethod]
        public void TestGetAll()
        {

            var all = _repo.GetAll();

            foreach (var r in all) {
                Debug.WriteLine(r);
            }

            Assert.IsNotNull(all);

        }

        [TestMethod]
        public void TestGetByProperty()
        {

            Reservation r = new Reservation()
            {
                Name = "Test Reservation",
                Body = "Reservation Body",
                TimeFrom = new DateTime(2021, 2, 24, 12, 0, 0),
                TimeTo = new DateTime(2021, 2, 24, 13, 0, 0),
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Token = DateTime.Now.Ticks.ToString(),
            };

            _repo.Create(r);

            var get = _repo.GetByProperty(Reservation.NAME, "Test Reservation");

            Debug.WriteLine(get);

            Assert.IsNotNull(get);

        }

        [TestMethod]
        public void TestGetWhere()
        {

            var get = _repo.GetWhere(new EqualConstraint(Reservation.NAME, "Test Reservation"));

            Debug.WriteLine(get.FirstOrDefault());

            Assert.IsNotNull(get);

        }

        [TestMethod]
        public void TestUpdate()
        {

            Reservation r = new Reservation()
            {
                Name = "Test Reservation",
                Body = "Reservation Body",
                TimeFrom = new DateTime(2021, 2, 24, 12, 0, 0),
                TimeTo = new DateTime(2021, 2, 24, 13, 0, 0),
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Token = DateTime.Now.Ticks.ToString(),
            };

            _repo.Create(r);

            var get = _repo.GetWhere(new EqualConstraint(Reservation.NAME, "Test Reservation")).FirstOrDefault();

            get.Name = "Updated Test Reservation";
            get.Modified = DateTime.Now;

            _repo.Update(get);

            Debug.WriteLine(get);

        }

        [TestMethod]
        public void TestDelete()
        {

            Reservation r = new Reservation()
            {
                Name = "Test Reservation",
                Body = "Reservation Body",
                TimeFrom = new DateTime(2021, 2, 24, 12, 0, 0),
                TimeTo = new DateTime(2021, 2, 24, 13, 0, 0),
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Token = DateTime.Now.Ticks.ToString(),
            };

            _repo.Create(r);

            var get = _repo.GetWhere(new EqualConstraint(Reservation.NAME, "Test Reservation"));

            _repo.Delete(get.FirstOrDefault());

        }

        [TestMethod]
        public void TestDeleteWhere()
        {

            Reservation r = new Reservation()
            {
                Name = "Test Reservation",
                Body = "Reservation Body",
                TimeFrom = new DateTime(2021, 2, 24, 12, 0, 0),
                TimeTo = new DateTime(2021, 2, 24, 13, 0, 0),
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Token = DateTime.Now.Ticks.ToString(),
            };

            _repo.Create(r);

            _repo.DeleteWhere(new EqualConstraint(Reservation.NAME, "Test Reservation"));

        }

    }
}
