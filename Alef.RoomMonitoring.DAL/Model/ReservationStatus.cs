using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Model
{
    public class ReservationStatus
    {

        public static readonly ReservationStatus UNCHECKED = new ReservationStatus
        { 
            Id = 1,
            Name = "Unchecked",
        };

        public static readonly ReservationStatus OK = new ReservationStatus
        {
            Id = 2,
            Name = "OK",
        };

        public static readonly ReservationStatus NOTIFIED = new ReservationStatus
        {
            Id = 3,
            Name = "Notified",
        };

        public int Id { get; set; }
        public string Name { get; set; }

        private ReservationStatus() { }

        public override string ToString()
        {
            return Name;
        }

    }
}
