using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.Test
{
    [TestClass]
    public class PersonRepositoryTest : TestBase
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var persRepo = serviceProvider.GetService<IPersonRepository>();

            var result = await persRepo.GetAll();

            Assert.IsNotNull(result);
        }
    }
}
