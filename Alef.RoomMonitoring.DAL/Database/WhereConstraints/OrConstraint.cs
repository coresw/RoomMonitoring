using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Database.WhereConstraints
{
    public class OrConstraint : IConstraint
    {

        private IConstraint[] _constraints;

        public OrConstraint(params IConstraint[] constraints)
        {
            _constraints = constraints;
        }

        public string BuildSQL()
        {
            string res = "";
            for (int i = 0; i < _constraints.Length; i++)
            {
                res += _constraints[i].BuildSQL();
                if (i < _constraints.Length - 1)
                {
                    res += " or ";
                }
            }
            return res;
        }
    }
}
