using Alef.RoomMonitoring.Service.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Alef.RoomMonitoring.Service.Services.Interfaces;
using Alef.RoomMonitoring.DAL.Services.Interfaces;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;

namespace Alef.RoomMonitoring.Test.Services
{
    [TestClass]
    public class MSGraphProviderTest: TestBase
    {

        [TestMethod]
        public async Task TestGet()
        {

            var roomRepo = serviceProvider.GetRequiredService<IRoomRepository>();
            var service = serviceProvider.GetService<IMSGraphProvider>();

            try
            {
                var room = await roomRepo.GetById(1);
                await service.GetUpcomingRoomReservations(room.EMail);
                Assert.IsTrue(true);
            }
            catch(Exception e)
            {
                throw e;
            }

        }

    }
}
