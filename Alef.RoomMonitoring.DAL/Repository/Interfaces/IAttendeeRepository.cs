using Alef.RoomMonitoring.DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Repository.Interfaces
{
    public interface IAttendeeRepository 
    {
        Task Create(Attendee a);
        Task<Attendee> GetById(int id);
        Task<Attendee> GetByPersonReservation(int personId, int reservationId);
        Task<IEnumerable<Attendee>> GetAll();
        Task Update(Attendee a);
        Task Delete(Attendee a);
    }
}
