using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Model
{
    public class Attendee
    {

        public int Id { get; set; }
        public static string ID = "Id";
        public int PersonId { get; set; }
        public static string PERSON_ID = "PersonId";
        public int ReservationId { get; set; }
        public static string RESERVATION_ID = "ReservationId";
        public int AttendeeTypeId { get; set; }
        public static string ATTENDEE_TYPE_ID = "AttendeeTypeId";

        public override string ToString()
        {
            return PersonId + "" + ReservationId + " " + AttendeeTypeId;
        }

    }
}
