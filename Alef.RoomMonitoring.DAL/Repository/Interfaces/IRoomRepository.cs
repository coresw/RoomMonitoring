using Alef.RoomMonitoring.DAL.Database.WhereConstraints;
using Alef.RoomMonitoring.DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Repository.Interfaces
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        void Create(Room r);
    }
}
