using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Model
{
    public class Attendee
    {

        public int Id { get; set; }
        public int PersonId { get; set; }
        public int ReservationId { get; set; }
        public int AttendeeTypeId { get; set; }

        public override string ToString()
        {
            return PersonId + "" + ReservationId + " " + AttendeeTypeId;
        }

    }
}
