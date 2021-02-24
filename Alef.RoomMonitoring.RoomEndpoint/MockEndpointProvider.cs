using Alef.RoomMonitoring.DAL.Model;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using CiscoEndpointProvider;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.RoomEndpoint
{
    public class MockEndpointProvider : IEndpointProvider
    {

        private readonly IMockEndpointRepository _mockEndpointRepo;

        public MockEndpointProvider(IMockEndpointRepository mockEndpointRepo) {
            _mockEndpointRepo = mockEndpointRepo;
        }

        public int GetPeopleCount(string endpointIP)
        {
            try
            {
                int peopleCount = _mockEndpointRepo.GetByProperty(MockEndpoint.ENDPOINT_IP, endpointIP).PeopleCount;

                return peopleCount;
            }
            catch
            {
                throw;
            }
        }
    }
}
