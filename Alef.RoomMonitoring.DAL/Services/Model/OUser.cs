using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Services.Model
{
    public class OUser
    {
        public string EmailAddress { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name + " " + EmailAddress;
        }

    }
}
