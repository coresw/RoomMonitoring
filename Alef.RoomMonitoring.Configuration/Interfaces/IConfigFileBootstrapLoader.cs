using Alef.RoomMonitoring.Configuration.ConfigFileSections;
using Alef.RoomMonitoring.Configuration.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.Configuration.Interfaces
{
    public interface IConfigFileBootstrapLoader
    {
        IList<JobSetting> GetJobSettings();

        DbConfiguration GetDbConfiguration();

        MSGraphSetting GetMSGraphSetting();
    }
}
