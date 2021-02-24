using Alef.RoomMonitoring.DAL.Database.WhereConstraints;
using Alef.RoomMonitoring.DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Repository.Interfaces
{
    public interface IMockEndpointRepository: IBaseRepository<MockEndpoint>
    {
        void Create(MockEndpoint e);
    }
}
