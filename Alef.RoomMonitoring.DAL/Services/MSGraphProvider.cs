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
                    DateTime.Today.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ") +
                    "&enddatetime=" + DateTime.Today.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                JArray roomReservs = (await _graphAPI.SendRequestAsync(query))["value"] as JArray;

                foreach (JObject obj in roomReservs) 
                {

                    // get organizers instance to get subject and body
                    string reservQuery = "/users/" +
                        obj["organizer"]["emailAddress"]["address"] +
                        "/events?$filter=iCalUId eq '" + obj["iCalUId"]+"'";

                    JObject reserv = (await _graphAPI.SendRequestAsync(reservQuery))["value"][0] as JObject;

                    OUser organizer = new OUser { 
                        Name = reserv["organizer"]["emailAddress"]["name"].ToString(),
                        EmailAddress = reserv["organizer"]["emailAddress"]["address"].ToString(),
                    };
                    IList<OUser> attendees = new List<OUser>();

                    foreach (JObject attendee in reserv["attendees"]) {

                        if (attendee["type"].ToString() == "required" || attendee["type"].ToString() == "optional")
                        {
                            attendees.Add(new OUser
                            {
                                Name = attendee["emailAddress"]["name"].ToString(),
                                EmailAddress = attendee["emailAddress"]["address"].ToString()
                            });
                        }

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
                _logger.Error(ex.Demystify(), "Failed fetching reservations");
                throw;
            }

            return result;

        }

    }
}
