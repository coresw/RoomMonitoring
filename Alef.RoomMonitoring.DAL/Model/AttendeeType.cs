using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Model
{
    public class AttendeeType
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public static readonly AttendeeType ORGANIZER = new AttendeeType
        {
            Id = 1,
            Name = "Organizer",
        };
        public static readonly AttendeeType REQUIRED = new AttendeeType
        {
            Id = 2,
            Name = "Required",
        };
        public static readonly AttendeeType OPTIONAL = new AttendeeType
        {
            Id = 3,
            Name = "Optional",
        };

        private AttendeeType() { 
            
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
