using Alef.RoomMonitoring.DAL.Services.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Services.Interfaces
{
    public interface IMSGraphProvider
    {
        Task<IEnumerable<OReservation>> GetReservationsAsync();
    }
}
