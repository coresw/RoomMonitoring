using Alef.RoomMonitoring.Service.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.Test.Services
{
    [TestClass]
    public class CheckReservationServiceTest: TestBase
    {

        private ICheckReservationService _service;

        public CheckReservationServiceTest() {

            _service = serviceProvider.GetService<ICheckReservationService>();

        }

        [TestMethod]
        public void TestCheck() {

            _service.CheckReservations();

        }

    }
}
