using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Database.WhereConstraints
{
    public class LogicConstraint: IConstraint
    {

        private IConstraint[] _constraints;
        private string _operator;

        protected LogicConstraint(string op, params IConstraint[] constraints) {

            _operator = op;
            _constraints = constraints;

        }

        public string Build(Dictionary<string, object> parameters) {

            string res = "";
            for (int i = 0; i < _constraints.Length; i++)
            {
                res += _constraints[i].Build(parameters);
                if (i < _constraints.Length - 1)
                {
                    res += " " + _operator + " ";
                }
            }
            return res;

        }

    }
}
