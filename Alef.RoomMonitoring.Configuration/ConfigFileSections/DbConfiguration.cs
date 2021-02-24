using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;

namespace Alef.RoomMonitoring.Configuration.ConfigFileSections
{
    public class DbConfiguration
    {
        public string Server { get; set; }
        public string Username { get; set; }
        //public SecureString Password { get; set; }
        public string Password { get; set; }
        public string DbName { get; set; }
        public bool UseWindowsAuthentication { get; set; }
        public string ConfigTableName { get; set; }

        #region Public static methods
        //public static DbConfiguration Read(Config config)
        //{
        //    if (config == null) throw new ArgumentNullException(nameof(config));
        //    SecureString secure = new SecureString();
        //    Array.ForEach(config.ReadConfig("Database", "Password").ToCharArray(), secure.AppendChar);

        //    return new DbConfiguration
        //    {
        //        DbName = config.ReadConfig("Database", "DbName"),
        //        Server = config.ReadConfig("Database", "IPAddress"),
        //        Username = config.ReadConfig("Database", "Username"),
        //        Password = secure,
        //        ConfigTableName = config.ReadConfig("Database", "TableName"),
        //        UseWindowsAuthentication = config.ReadConfigAttrBool("Database", "UseWindowsAuthentication")
        //    };
        //}
        #endregion

        #region Public methods
        public override string ToString()
        {
            return $"Server: {Server}, Username: {Username}, Password: {new string('*', Password.Length)}, DbName: {DbName}, UseWindowsAuthentication: {UseWindowsAuthentication}, ConfigTableName: {ConfigTableName}";
        }

        public string ToConnectionString()
        {
            if (UseWindowsAuthentication)
                return $"data source={Server};initial catalog={DbName};integrated security=true;persist security info=True;";

            return $"data source={Server};initial catalog={DbName};integrated security=false;persist security info=True;" +
                   $"User ID={Username};Password={new NetworkCredential("", Password).Password}";
        }


        public string ToMaskedStringForLog()
        {
            if (UseWindowsAuthentication)
                return $"data source={Server};initial catalog={DbName};integrated security=true;persist security info=True;";

            return $"data source={Server};initial catalog={DbName};integrated security=false;persist security info=True;" +
                   $"User ID={Username};Password={new string('*', Password.Length)}";
        }

        private string GetAttribute(string line)
        {
            if (!string.IsNullOrEmpty(line))
            {
                return line.Split(".").LastOrDefault();
            }

            return null;
        }
        #endregion

        #region Helper class
        public class AppSettingsObject
        {
            public string XPath { get; set; }
            public string Value { get; set; }
        }
        #endregion
    }
}
