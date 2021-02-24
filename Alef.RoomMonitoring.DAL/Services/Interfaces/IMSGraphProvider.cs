using Alef.RoomMonitoring.DAL.Services.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Services.Interfaces
{
    public interface IMSGraphProvider
    {
        Task<IEnumerable<OReservation>> GetUpcomingRoomReservations(string roomEmail);
        Task SendMail(string from, string subject, string body, IEnumerable<string> to);
    }
}
