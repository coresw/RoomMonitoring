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
    public class CheckReservationServiceTest: TestBase
    {

        [TestMethod]
        public async Task TestCheck()
        {

            var service = serviceProvider.GetService<ICheckReservationService>();

            try
            {
                await service.CheckReservations();
                Assert.IsTrue(true);
            }
            catch(Exception e)
            {
                throw e;
            }

        }

    }
}
