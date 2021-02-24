using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Database.WhereConstraints
{
    public class OrConstraint : LogicConstraint
    {

        public OrConstraint(params IConstraint[] constraints) : base("or", constraints) { }

    }
}
