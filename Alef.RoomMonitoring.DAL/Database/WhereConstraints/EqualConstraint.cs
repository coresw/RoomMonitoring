using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Database.WhereConstraints
{
    public class EqualConstraint : ComparisonConstraint
    {
        public EqualConstraint(string name, object value) : base(name, value, "=") {}
    }
}
