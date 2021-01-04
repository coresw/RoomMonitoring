using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Threading.Tasks;

namespace CiscoEndpointProvider
{
    public class EndpointProvider : IEndpointProvider
    {
        public int GetPeopleCount(string endpointIP)
        {
            try
            {
                var client = new RestClient($"http://{endpointIP}/getxml?location=/Status/RoomAnalytics/PeopleCount");
                client.Authenticator = new HttpBasicAuthenticator("API", "DevRocks!");
                //client.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection() { };
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                //
                IRestResponse response = client.Execute(request);
                //

                int peopleCount = 1; //TODO bud nahrazeno parsovanim response
                return(peopleCount);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
