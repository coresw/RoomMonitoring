using Alef.RoomMonitoring.DAL.Database.WhereConstraints;
using Alef.RoomMonitoring.DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Repository.Interfaces
{
    public interface IRoomRepository 
    {
        void Create(Room r);
        Room GetById(int id);
        Room GetByEMail(string email);
        IEnumerable<Room> GetAll();
        IEnumerable<Room> GetWhere(IConstraint constraint);
        void Update(Room r);
        void Delete(Room r);
        void DeleteWhere(IConstraint constraint);
    }
}
