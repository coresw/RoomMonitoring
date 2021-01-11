using Alef.RoomMonitoring.DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Repository.Interfaces
{
    public interface IRoomRepository 
    {
        Task Create(Room r);
        Task<Room> GetById(int id);
        Task<Room> GetByEMail(string email);
        Task<IEnumerable<Room>> GetAll();
        Task Update(Room r);
        Task Delete(Room r);
    }
}
