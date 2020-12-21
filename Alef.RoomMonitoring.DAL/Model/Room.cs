using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Model
{
    public class Room
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string EndpointIP { get; set; }
        public bool Occupied { get; set; }

        public override string ToString()
        {
            return Name + " " + Occupied;
        }

    }
}
