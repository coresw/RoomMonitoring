using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Model
{
    public class MockEndpoint
    {

        public int Id { get; set; }
        public static string ID = "Id";

        public string EndpointIp { get; set; }
        public static string ENDPOINT_IP = "EndpointIp";

        public int PeopleCount { get; set; }
        public static string PEOPLE_COUNT = "PeopleCount";

    }
}
