using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Database.WhereConstraints
{
    public class NotEqualConstraint : ComparisonConstraint
    {

        public NotEqualConstraint(string name, object value) : base(name, value, "!=") { }

    }
}
