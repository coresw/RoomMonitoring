using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Model
{
    public class ReservationStatus
    {

        public static readonly ReservationStatus PLANNED = new ReservationStatus
        { 
            Id = 1,
            Name = "PLANNED",
            Display = "Planned",
        };

        public static readonly ReservationStatus ACTIVE = new ReservationStatus
        {
            Id = 2,
            Name = "ACTIVE",
            Display = "Active",
        };

        public static readonly ReservationStatus EMPTY = new ReservationStatus
        {
            Id = 3,
            Name = "EMPTY",
            Display = "Room Empty",
        };
        public static readonly ReservationStatus CLOSED = new ReservationStatus
        {
            Id = 4,
            Name = "CLOSED",
            Display = "Closed",
        };

        public static ReservationStatus GetById(int id) {
            switch (id) {
                case 1:
                    return PLANNED;
                case 2:
                    return ACTIVE;
                case 3:
                    return EMPTY;
                case 4:
                    return CLOSED;
            }
            return null;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Display { get; set; }

        private ReservationStatus() { }

        public override string ToString()
        {
            return Name;
        }

    }
}
