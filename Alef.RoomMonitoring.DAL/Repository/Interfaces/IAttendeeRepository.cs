using Alef.RoomMonitoring.DAL.Database.WhereConstraints;
using Alef.RoomMonitoring.DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Repository.Interfaces
{
    public interface IAttendeeRepository 
    {
        void Create(Attendee a);
        Attendee GetById(int id);
        Attendee GetByPersonReservation(int personId, int reservationId);
        IEnumerable<Attendee> GetAll();
        IEnumerable<Attendee> GetWhere(IConstraint constraint);
        void Update(Attendee a);
        void Delete(Attendee a);
        void DeleteWhere(IConstraint constraint);
    }
}
