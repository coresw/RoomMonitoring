using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Database.WhereConstraints
{
    public class ComparisonConstraint: IConstraint
    {

        private string _name;
        private object _value;
        private string _operator;

        protected ComparisonConstraint(string name, object value, string op) {

            _name = name;
            _value = value;
            _operator = op;

        }

        public string Build(Dictionary<string, object> parameters) {

            string name = _name;
            int i = 0;

            while (parameters.ContainsKey(name)) {
                name = _name + i;
            }

            parameters.Add(name, _value);

            return name + _operator + "@" + name;

        }

    }
}
