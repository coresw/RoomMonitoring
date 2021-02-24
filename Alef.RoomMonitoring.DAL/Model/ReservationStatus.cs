using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Model
{
    public class ReservationStatus
    {

        public int Id { get; set; }
        public static string ID = "Id";
        public string Name { get; set; }
        public static string NAME = "Name";
        public string Display { get; set; }
        public static string DISPLAY = "Display";

        public static string PLANNED = "PLANNED";
        public static string ACTIVE = "ACTIVE";
        public static string EMPTY = "EMPTY";
        public static string CLOSED = "CLOSED";

        public ReservationStatus() { }

        public override string ToString()
        {
            return Display;
        }

    }
}
