using Alef.RoomMonitoring.DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Repository.Interfaces
{
    public interface IReservationRepository 
    {
        Task Create(Reservation r);
        Task<Reservation> GetById(int id);
        Task<Reservation> GetByToken(string token);
        Task<IEnumerable<Reservation>> GetAll();
        Task<IEnumerable<Reservation>> GetWhere(string token = null, DateTime created = new DateTime(), DateTime modified = new DateTime(),
            DateTime timeFrom = new DateTime(), DateTime timeTo = new DateTime(), string name = null, string body = null,
            int reservationStatusId = -1, int roomId = -1);
        Task Update(Reservation r);
        Task Delete(Reservation r);
    }
}
