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
                    Console.WriteLine(e);
                    _logger.Error(e.Demystify(), $"Failed parsing response: "+e.Message);
                    throw;
                }
            }
            else
            {
                Console.WriteLine(response.StatusCode+"\n"+content);
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
