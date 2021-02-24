using Alef.RoomMonitoring.Service.Services;
using Alef.RoomMonitoring.Service.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.Test.Services
{
    [TestClass]
    public class ReservationSyncServiceTest: TestBase
    {

        private IReservationSyncService _service;

        public ReservationSyncServiceTest() {

            _service = serviceProvider.GetService<IReservationSyncService>();

        }

        [TestMethod]
        public void TestSync() {

            _service.SyncReservations().Wait();

        }

    }
}
