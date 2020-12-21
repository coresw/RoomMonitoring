using Alef.RoomMonitoring.Configuration;
using Alef.RoomMonitoring.DAL.Database;
using Alef.RoomMonitoring.DAL.Database.Interfaces;
using Alef.RoomMonitoring.DAL.Model;
using Alef.RoomMonitoring.DAL.Services;
using Alef.RoomMonitoring.DAL.Services.Interfaces;
using Alef.RoomMonitoring.DAL.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Repository
{
    class TMP_Test
    {

        public static void Main(string[] args) {

            MSGraphAPI api = new MSGraphAPI(new ConfigFileBootstrapLoader());

            MSGraphProvider prov = new MSGraphProvider(api);

            IEnumerable<OReservation> reservs = prov.GetReservations().Result;

            foreach (OReservation r in reservs) {
                Console.WriteLine(r);
            }

            //Console.WriteLine(api.SendRequestAsync("/users/roommon@sqertx.onmicrosoft.com/events").Result["value"]);

        }

    }
}
