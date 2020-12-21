using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Services.Model
{
    public class OReservation
    {
        public string Id { get; set; }
        
        public string Name { get; set; }

        public string Body { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public DateTime TimeFrom { get; set; }

        public DateTime TimeTo { get; set; }

        public IList<OUser> Attendees { get; set; }

        public OUser Organizer { get; set; }

        public string Location { get; set; }

        public override string ToString()
        {
            return Name + " " + Body + " " + TimeFrom + " " + TimeTo + " " + Organizer + " " + Location;
        }

    }
}
