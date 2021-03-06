﻿using Alef.RoomMonitoring.Configuration.ConfigFileSections;
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

            return new List<JobSetting> {
                new JobSetting() {
                    Name= "CheckReservationJob",
                    Enabled=true,
                    SimpleTriggerTime = 300
                },
                new JobSetting() {
                    Name= "ReservationSyncJob",
                    Enabled=true,
                    SimpleTriggerTime = 300
                },
                new JobSetting() {
                    Name= "RoomStatusSyncJob",
                    Enabled=true,
                    SimpleTriggerTime = 300
                },
            };
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

        public ReservationSettings GetReservationSettings()
        {
            return new ReservationSettings {
                CheckTimeout = 15,
            };
        }
    }
}
