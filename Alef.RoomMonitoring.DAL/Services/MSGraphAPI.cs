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

        private string _accessToken;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IConfigFileBootstrapLoader _configLoader;

        public MSGraphAPI(IConfigFileBootstrapLoader configLoader) 
        {
            _configLoader = configLoader;
        }

        public async Task<JObject> SendRequestAsync(string request) 
        {

            string accessToken = GetAccessToken();

            var setting = _configLoader.GetMSGraphSetting();

            string url = setting.Url + "v1.0/" + request;

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

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
                    _logger.Error(e.Demystify(), "Failed parsing response");
                    throw;
                }
            }
            else
            {
                _logger.Error(response.StatusCode+" "+content);
                throw new Exception("Failed processing request: "+response.StatusCode+" "+content);
            }

        }

        private string GetAccessToken() 
        {
            try
            {

                if (_accessToken == null)
                {

                    var setting = _configLoader.GetMSGraphSetting();

                    IConfidentialClientApplication authenticator = ConfidentialClientApplicationBuilder
                            .Create(setting.ClientID)
                            .WithClientSecret(setting.ClientSecret)
                            .WithAuthority(setting.Authority)
                            .Build();

                    AuthenticationResult result;
                    result = authenticator.AcquireTokenForClient(new string[] { setting.Url + ".default" }).ExecuteAsync().Result;
                    _accessToken = result.AccessToken;

                }

                return _accessToken;
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Demystify(), "Authentication failed");
                throw;
            }
        }

    }
}
