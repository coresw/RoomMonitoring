using Alef.RoomMonitoring.DAL.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Repository.Interfaces
{
    public interface IBaseRepository
    {

        IDBProvider Database { get; }

    }
}
