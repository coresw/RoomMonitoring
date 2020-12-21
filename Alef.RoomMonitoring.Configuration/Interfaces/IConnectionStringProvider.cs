using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.Configuration.Interfaces
{
    public interface IConnectionStringProvider
    {
        string Value { get; }
        string Value2 { get; }
    }
}
