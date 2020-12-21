using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Model
{
    public class Person
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Person person &&
                   EMail == person.EMail;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EMail);
        }

        public override string ToString()
        {
            return Name + " " + EMail;
        }

    }
}
