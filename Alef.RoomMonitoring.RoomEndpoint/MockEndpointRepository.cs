using Alef.RoomMonitoring.DAL.Database.Interfaces;
using Alef.RoomMonitoring.DAL.Database.WhereConstraints;
using Alef.RoomMonitoring.DAL.Model;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Repository
{
    public class MockEndpointRepository: BaseRepository<MockEndpoint>, IMockEndpointRepository
    {

        public MockEndpointRepository(IDBProvider database) : base(database)
        {

            TableName = "MockEndpoint";
            IdentityField = MockEndpoint.ID;
            Fields = new string[] {
                MockEndpoint.ENDPOINT_IP, MockEndpoint.PEOPLE_COUNT
            };

        }

        public void Create(MockEndpoint e)
        {

            e.Id = base.Create<int>(e);

        }

    }
}
