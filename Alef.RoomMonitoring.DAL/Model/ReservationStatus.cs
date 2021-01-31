using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Model
{
    public class ReservationStatus
    {

        public static readonly ReservationStatus UNKNOWN = new ReservationStatus
        { 
            Id = 1,
            Name = "Unknown",
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
        public static readonly ReservationStatus UPCOMING = new ReservationStatus
        {
            Id = 4,
            Name = "Upcoming",
        };
        public static readonly ReservationStatus PAST = new ReservationStatus
        {
            Id = 5,
            Name = "Past",
        };

        public static ReservationStatus GetById(int id) {
            switch (id) {
                case 1:
                    return UNKNOWN;
                case 2:
                    return OK;
                case 3:
                    return NOTIFIED;
                case 4:
                    return UPCOMING;
                case 5:
                    return PAST;
            }
            return null;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        private ReservationStatus() { }

        public override string ToString()
        {
            return Name;
        }

    }
}
