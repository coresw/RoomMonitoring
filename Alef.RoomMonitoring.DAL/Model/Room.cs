using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Model
{
    public class Room
    {

        public int Id { get; set; }
        public static string ID = "Id";
        public string Name { get; set; }
        public static string NAME = "Name";
        public string EMail { get; set; }
        public static string EMAIL = "EMail";
        public string EndpointIP { get; set; }
        public static string ENDPOINT_IP = "EndpointIp";

        public override string ToString()
        {
            return Name + " " + EMail;
        }

    }
}
