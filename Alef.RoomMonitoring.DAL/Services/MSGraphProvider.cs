using Alef.RoomMonitoring.DAL.Services.Interfaces;
using Alef.RoomMonitoring.DAL.Services.Model;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Services
{
    public class MSGraphProvider : IMSGraphProvider
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IMSGraphAPI _graphAPI;

        public MSGraphProvider(IMSGraphAPI graphAPI)
        {
            _graphAPI = graphAPI;
        }

        public async Task<IEnumerable<OReservation>> GetReservationsAsync()
        {
            try
            {
                //Metoda nacte rezervace jednotlivych mistnosti

                //vrati kolekci OReservation tzn. pres graphAPI ziska odpoved z O365
                //Data z O365 rozparsovat a dat do kolekce

                Dictionary<string, OUser> users = await getUsersAsync();

                Dictionary<string, OReservation> res = new Dictionary<string, OReservation>(); // all reservations, uniquely identified by their IDs

                foreach (OUser user in users.Values)
                {

                    string request = "/users/" + user.EmailAddress + "/events";

                    JArray events = (await _graphAPI.SendRequestAsync(request))["value"] as JArray; // all events of this user

                    foreach (JObject o in events)
                    {

                        string id = o["iCalUId"].ToString();

                        if (res.ContainsKey(id)) continue; // skip already loaded events

                        OReservation reserv = new OReservation {
                            Id = id,
                            Name = o["subject"].ToString(),
                            Body = o["bodyPreview"].ToString(),
                            Created = DateTime.Parse(o["createdDateTime"].ToString()),
                            Modified = DateTime.Parse(o["lastModifiedDateTime"].ToString()),
                            TimeFrom = DateTime.Parse(o["start"]["dateTime"].ToString()),
                            TimeTo = DateTime.Parse(o["end"]["dateTime"].ToString()),
                            Location = o["location"]["displayName"].ToString(),
                            Attendees = new List<OUser>(),
                            Organizer = users[o["organizer"]["emailAddress"]["address"].ToString()],
                        };

                        foreach (JObject a in o["attendees"])
                        {
                            reserv.Attendees.Add(users[a["emailAddress"]["address"].ToString()]);
                        }

                        res.Add(id, reserv);

                    }

                }

                return res.Values;

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Demystify(), "Failed fetching reservations: "+ex.Message);
                throw;
            }
        }

        private async Task<Dictionary<string, OUser>> getUsersAsync()
        {
            try
            {

                Dictionary<string, OUser> res = new Dictionary<string, OUser>(); // all users, uniquely identified by their mail addresses

                string request = "/users/";

                JArray users = (await _graphAPI.SendRequestAsync(request))["value"] as JArray;

                foreach (JObject o in users)
                {

                    OUser user = new OUser
                    {
                        Name = o["displayName"].ToString(),
                        EmailAddress = o["mail"].ToString(),
                    };

                    res.Add(user.EmailAddress, user);

                }

                return res;

            }
            catch (Exception e) {
                _logger.Error(e.Demystify(), "Failed fetching users: "+e.Message);
                throw;
            }
        }

    }
}
