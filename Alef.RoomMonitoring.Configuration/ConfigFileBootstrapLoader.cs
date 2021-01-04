using Alef.RoomMonitoring.Configuration.ConfigFileSections;
using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.Configuration.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.Configuration
{
    public class ConfigFileBootstrapLoader : IConfigFileBootstrapLoader
    {
        public IList<JobSetting> GetJobSettings()
        {
            JobSetting job1 = new JobSetting() {
                Name= "GetRoomReservationJob",
                Enabled=true,
                SimpleTriggerTime = 300
            };

            IList<JobSetting> result = new List<JobSetting>();
            result.Add(job1);

            return (result);
        }

        public DbConfiguration GetDbConfiguration()
        {
            var dbCfg = new DbConfiguration() {
                DbName = "RoomMonitoring",
                Server = "sql.develop.local",
                Username = "sa",
                Password = "cisco",
                ConfigTableName = String.Empty,
                UseWindowsAuthentication = false
            };

            var local = new DbConfiguration()
            {
                Server = "localhost",
                DbName = "RoomMonitoring",
                ConfigTableName = String.Empty,
                UseWindowsAuthentication = true,
                DateFormat = "yyyy-MM-ddTHH:mm:ss",
            };

            return local;
            //return DbConfiguration.Read(xmlConfig);
        }


        public MSGraphSetting GetMSGraphSetting()
        {
            MSGraphSetting result = new MSGraphSetting()
            {
                Url = "https://graph.microsoft.com/",
                Authority = "https://login.microsoftonline.com/145c41ec-a970-494e-9d17-79a48d240622",
                ClientID = "61423d68-1fd7-4e93-8638-73ecf51f4455",
                ClientSecret = "9UT_etWRo1Sa7_k13Q-~76_g_4B63hzd-E"
            };

            return (result);
        }

    }
}
