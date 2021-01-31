using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Database.WhereConstraints
{
    public interface IConstraint
    {
        string BuildSQL();
    }
}
