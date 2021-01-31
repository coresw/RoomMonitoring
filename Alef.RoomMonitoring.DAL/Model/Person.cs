using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Model
{
    public class Person
    {

        public int Id { get; set; }
        public static string ID = "Id";
        public string Name { get; set; }
        public static string NAME = "Name";
        public string EMail { get; set; }
        public static string EMAIL = "EMail";

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
