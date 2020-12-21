using Alef.RoomMonitoring.DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Repository.Interfaces
{
    public interface IReservationStatusRepository
    {
        Task<ReservationStatus> GetById(int id);
        Task<ReservationStatus> GetByName(string name);
        Task<IEnumerable<ReservationStatus>> GetAll();
    }
}
