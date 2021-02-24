using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.DAL.Database.Interfaces;
using Alef.RoomMonitoring.DAL.Database.WhereConstraints;
using Alef.RoomMonitoring.DAL.Model;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Repository
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        
        public RoomRepository(IDBProvider database) : base(database)
        {

            TableName = "Room";
            IdentityField = Room.ID;
            Fields = new string[] { 
                Room.NAME, Room.EMAIL, Room.ENDPOINT_IP
            };

        }

        public void Create(Room r)
        {
            r.Id = base.Create<int>(r);
        }

    }
}
