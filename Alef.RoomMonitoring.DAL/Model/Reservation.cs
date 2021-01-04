using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Model
{
    public class Reservation
    {

        public int Id { get; set; }
        public string Token { get; set; } // ID of this reservation in MS Office
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public int ReservationStatusId { get; set; } = 1;
        public int RoomId { get; set; } = 1;

        public override string ToString()
        {
            return Name + " " + Body + " " + Created + " " + Modified + " " + TimeFrom + " " + TimeTo;
        }

    }
}
