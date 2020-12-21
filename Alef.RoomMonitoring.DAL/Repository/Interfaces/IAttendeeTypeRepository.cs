using Alef.RoomMonitoring.DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Repository.Interfaces
{
    public interface IAttendeeTypeRepository 
    {
        Task<AttendeeType> GetById(int id);
        Task<AttendeeType> GetByName(string name);
        Task<IEnumerable<AttendeeType>> GetAll();
    }
}
