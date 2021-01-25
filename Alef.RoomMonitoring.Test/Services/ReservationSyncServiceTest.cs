using Alef.RoomMonitoring.Service.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Alef.RoomMonitoring.Service.Services.Interfaces;

namespace Alef.RoomMonitoring.Test.Services
{
    [TestClass]
    public class ReservationSyncServiceTest: TestBase
    {

        [TestMethod]
        public async Task TestSync()
        {

            var service = serviceProvider.GetService<IReservationSyncService>();

            try
            {
                await service.SyncReservations();
                Assert.IsTrue(true);
            }
            catch(Exception e)
            {
                throw e;
            }

        }

    }
}
