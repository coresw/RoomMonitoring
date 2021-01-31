using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Model
{
    public class Reservation
    {

        public int Id { get; set; }
        public static string ID = "Id";
        public string Token { get; set; } // ID of this reservation in MS Office
        public static string TOKEN = "Token";
        public DateTime Created { get; set; }
        public static string CREATED = "Created";
        public DateTime Modified { get; set; }
        public static string MODIFIED = "Modified";
        public DateTime TimeFrom { get; set; }
        public static string TIME_FROM = "TimeFrom";
        public DateTime TimeTo { get; set; }
        public static string TIME_TO = "TimeTo";
        public string Name { get; set; }
        public static string NAME = "Name";
        public string Body { get; set; }
        public static string BODY = "Body";
        public int ReservationStatusId { get; set; } = 1;
        public static string RESERVATION_STATUS_ID = "ReservationStatusId";
        public int RoomId { get; set; } = 1;
        public static string ROOM_ID = "RoomId";

        public override string ToString()
        {
            return Name + " " + Body + " " + Created + " " + Modified + " " + TimeFrom + " " + TimeTo;
        }

    }
}
