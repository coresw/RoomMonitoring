using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Model
{
    public class AttendeeType
    {

        public int Id { get; set; }
        public static string ID = "Id";
        public string Name { get; set; }
        public static string NAME = "Name";

        public static string ORGANIZER = "ORGANIZER";
        public static string REQUIRED = "REQUIRED";

        public override string ToString()
        {
            return Name;
        }

    }
}
