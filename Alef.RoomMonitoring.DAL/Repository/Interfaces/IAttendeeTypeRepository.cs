using Alef.RoomMonitoring.DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Repository.Interfaces
{
    public interface IAttendeeTypeRepository: IBaseRepository<AttendeeType>
    {
        void Create(AttendeeType a);
    }
}
