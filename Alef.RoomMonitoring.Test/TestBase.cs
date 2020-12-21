using Alef.RoomMonitoring.Configuration;
using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.DAL.Repository;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Alef.RoomMonitoring.DAL.Services;
using Alef.RoomMonitoring.DAL.Services.Interfaces;
using fm.Extensions.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Alef.RoomMonitoring.Test
{
    public class TestBase : ServiceTestsBase
    {
        protected ServiceProvider serviceProvider { get; set; }

        public TestBase()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IConfigFileBootstrapLoader, ConfigFileBootstrapLoader>();
            services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IAttendeeRepository, AttendeeRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IAttendeeTypeRepository, AttendeeTypeRepository>();
            services.AddScoped<IMSGraphAPI, MSGraphAPI>();
            services.AddLogging();

            serviceProvider = services.BuildServiceProvider();
        }
    }
}
