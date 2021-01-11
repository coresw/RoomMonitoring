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
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IMSGraphAPI _graphAPI;

        public MSGraphProvider(IMSGraphAPI graphAPI)
        {
            _graphAPI = graphAPI;
        }

        public async Task<IEnumerable<OReservation>> GetUpcomingRoomReservations(string roomEmail)
        {

            List<OReservation> result = new List<OReservation>();

            try {

                string query = "/users/" + roomEmail +
                    "/calendarView/delta?startdatetime=" +
                    DateTime.Today.ToString("yyyy-MM-ddTHH:mm:ss") +
                    "&enddatetime=" + DateTime.Today.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss");

                JArray roomReservs = _graphAPI.SendRequestAsync(query).Result["value"] as JArray;

                foreach (JObject obj in roomReservs) 
                {

                    // get organizers instance to get subject and body
                    string reservQuery = "/users/" +
                        obj["organizer"]["emailAddress"]["address"] +
                        "/events?$filter=iCalUId eq '" + obj["iCalUId"]+"'";

                    JObject reserv = _graphAPI.SendRequestAsync(reservQuery).Result["value"][0] as JObject;

                    OUser organizer = new OUser { 
                        Name = reserv["organizer"]["emailAddress"]["name"].ToString(),
                        EmailAddress = reserv["organizer"]["emailAddress"]["address"].ToString(),
                    };
                    IList<OUser> attendees = new List<OUser>();

                    foreach (JObject attendee in reserv["attendees"]) {

                        attendees.Add(new OUser { 
                            Name = attendee["emailAddress"]["name"].ToString(),
                            EmailAddress = attendee["emailAddress"]["address"].ToString()
                        });

                    }

                    result.Add(new OReservation
                    {
                        Id = reserv["iCalUId"].ToString(),
                        Name = reserv["subject"].ToString(),
                        Body = reserv["bodyPreview"].ToString(),
                        Created = DateTime.Parse(reserv["createdDateTime"].ToString()),
                        Modified = DateTime.Parse(reserv["lastModifiedDateTime"].ToString()),
                        TimeFrom = DateTime.Parse(reserv["start"]["dateTime"].ToString()),
                        TimeTo = DateTime.Parse(reserv["end"]["dateTime"].ToString()),
                        Organizer = organizer,
                        Attendees = attendees,
                    }) ;

                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Demystify(), "Failed fetching reservations: " + ex.Message);
                throw;
            }

            return result;

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
