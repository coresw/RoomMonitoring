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

namespace Alef.RoomMonitoring.DAL
{
    class TMP_Test
    {

        public static void Main(string[] args) {

            MSGraphAPI api = new MSGraphAPI(new ConfigFileBootstrapLoader());

            /*Console.WriteLine(api.SendRequestAsync("/users/TestRoom1@sqertx.onmicrosoft.com/calendarView/delta?startdatetime="+
                DateTime.Today.ToString("yyyy-MM-ddTHH:mm:ss")+"&enddatetime="+DateTime.Today.AddDays(99).ToString("yyyy-MM-ddTHH:mm:ss") +""
                ).Result);*/

            //Console.WriteLine(api.SendRequestAsync("/users/TestRoom1@sqertx.onmicrosoft.com/events/").Result);
            //Console.WriteLine(api.SendRequestAsync("/users/RoomMon@sqertx.onmicrosoft.com/events?$filter=iCalUid eq '040000008200E00074C5B7101A82E008000000002C22D55C54E7D601000000000000000010000000E9081C923F6582418B7AD7941297FB7B'").Result);

            MSGraphProvider prov = new MSGraphProvider(api);

            IEnumerable<OReservation> reservs = prov.GetUpcomingRoomReservations("TestRoom1@sqertx.onmicrosoft.com").Result;

            foreach (OReservation r in reservs) {
                Console.WriteLine(r);
            }

            //Console.WriteLine(api.SendRequestAsync("/users/roommon@sqertx.onmicrosoft.com/events").Result["value"]);

        }

    }
}
