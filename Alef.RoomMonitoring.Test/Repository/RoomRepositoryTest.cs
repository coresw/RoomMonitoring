using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.Test.Repository
{
    [TestClass]
    public class RoomRepositoryTest : TestBase
    {
        [TestMethod]
        public async Task TestGet()
        {

            var repo = serviceProvider.GetService<IRoomRepository>();

            var result = repo.GetAll();

            Assert.IsNotNull(result);

        }
    }
}
