using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Database.WhereConstraints
{
    public class NotEqualConstraint : IConstraint
    {

        private string _name;
        private string _value;

        public NotEqualConstraint(string name, object value) {
            _name = name;
            _value = value.ToString();
        }

        public string BuildSQL()
        {
            return _name + "!='" + _value + "'";
        }

    }
}
