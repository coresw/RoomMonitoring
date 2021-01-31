using Alef.RoomMonitoring.DAL.Database.WhereConstraints;
using Alef.RoomMonitoring.DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Repository.Interfaces
{
    public interface IReservationRepository 
    {
        void Create(Reservation r);
        Reservation GetById(int id);
        Reservation GetByToken(string token);
        IEnumerable<Reservation> GetAll();
        IEnumerable<Reservation> GetWhere(IConstraint constraint);
        void Update(Reservation r);
        void Delete(Reservation r);
        void DeleteWhere(IConstraint constraint);
    }
}
