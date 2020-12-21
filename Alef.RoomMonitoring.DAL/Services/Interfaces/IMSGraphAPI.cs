using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Services.Interfaces
{
    public interface IMSGraphAPI
    {
        Task<JObject> SendRequestAsync(string request);
    }
}
