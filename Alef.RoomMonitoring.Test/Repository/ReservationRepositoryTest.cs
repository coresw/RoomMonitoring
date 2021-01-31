using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.Test.Repository
{
    [TestClass]
    public class ReservationRepositoryTest : TestBase
    {
        [TestMethod]
        public async Task TestGet()
        {

            var repo = serviceProvider.GetService<IReservationRepository>();

            var result = repo.GetAll();

            Assert.IsNotNull(result);

        }
    }
}
