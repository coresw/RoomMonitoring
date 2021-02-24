using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Database.WhereConstraints
{
    public class AndConstraint : LogicConstraint
    {

        public AndConstraint(params IConstraint[] constraints): base("and", constraints) {}

    }
}
