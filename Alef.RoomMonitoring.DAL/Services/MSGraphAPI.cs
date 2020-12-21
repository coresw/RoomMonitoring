using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.DAL.Services.Interfaces;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Services
{
    public class MSGraphAPI : IMSGraphAPI
    {

        // TODO: load from config file
        //const string URL = "https://graph.microsoft.com/";
        //const string AUTHORITY = "https://login.microsoftonline.com/145c41ec-a970-494e-9d17-79a48d240622";
        //const string CLIENT_ID = "61423d68-1fd7-4e93-8638-73ecf51f4455";
        //const string CLIENT_SECRET = "9UT_etWRo1Sa7_k13Q-~76_g_4B63hzd-E";

        private string AccessToken;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IConfigFileBootstrapLoader _configLoader;

        #region Ctor

        public MSGraphAPI(IConfigFileBootstrapLoader configLoader) 
        {
            _configLoader = configLoader;
        }

        #endregion

        public async Task<JObject> SendRequestAsync(string request) 
        {

            if (AccessToken == null) 
            {
                authenticate();
            }

            var setting = _configLoader.GetMSGraphSetting();

            string url = setting.Url + "v1.0/" + request;

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", AccessToken);

            HttpResponseMessage response = await client.GetAsync(url);

            string content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return JObject.Parse(content);
                }
                catch (Exception e) 
                {
                    _logger.Error(e.Demystify(), $"Failed parsing response: "+e.Message);
                    throw;
                }
            }
            else
            {
                _logger.Error($"Failed calling API: status={response.StatusCode}");
                return null;
            }

        }

        private void authenticate() 
        {
            try
            {
                var setting = _configLoader.GetMSGraphSetting();

                IConfidentialClientApplication authenticator = ConfidentialClientApplicationBuilder
                        .Create(setting.ClientID)
                        .WithClientSecret(setting.ClientSecret)
                        .WithAuthority(setting.Authority)
                        .Build();

                AuthenticationResult result;
                    result = authenticator.AcquireTokenForClient(new string[] { setting.Url + ".default" }).ExecuteAsync().Result;
                    AccessToken = result.AccessToken;
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Demystify(), $"Failed acquiring auth token: ex={ex.Message}");
                throw;
            }
        }

    }
}
