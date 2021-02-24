using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alef.RoomMonitoring.Configuration;
using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.DAL.Database;
using Alef.RoomMonitoring.DAL.Database.Interfaces;
using Alef.RoomMonitoring.DAL.Repository;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Alef.RoomMonitoring.DAL.Services;
using Alef.RoomMonitoring.DAL.Services.Interfaces;
using Alef.RoomMonitoring.RoomEndpoint;
using CiscoEndpointProvider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Alef.RoomMonitoring.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddSingleton<IConfigFileBootstrapLoader, ConfigFileBootstrapLoader>();

            services.AddSingleton<IDBProvider, DBProvider>();
            services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();

            services.AddSingleton<IReservationRepository, ReservationRepository>();
            services.AddSingleton<IReservationStatusRepository, ReservationStatusRepository>();
            services.AddSingleton<IRoomRepository, RoomRepository>();
            services.AddSingleton<IPersonRepository, PersonRepository>();
            services.AddSingleton<IAttendeeRepository, AttendeeRepository>();
            services.AddSingleton<IAttendeeTypeRepository, AttendeeTypeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
