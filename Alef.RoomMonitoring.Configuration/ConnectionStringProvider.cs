using Alef.RoomMonitoring.Configuration.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.Configuration
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        public string Value { get; }
        public string Value2 { get; }

        public ConnectionStringProvider(IConfigFileBootstrapLoader configFileLoader)
        {
            Value = configFileLoader.GetDbConfiguration().ToConnectionString();
            //Value2 = configFileLoader.GetDbConfiguration().ToServiceDeskConnectionString();
        }
    }
}
