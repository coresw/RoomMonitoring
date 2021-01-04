﻿using Alef.RoomMonitoring.DAL.Model;
using Alef.RoomMonitoring.DAL.Repository;
using Alef.RoomMonitoring.Service.Services.Interfaces;
using CiscoEndpointProvider;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.Service.Services
{
    public class RoomStatusSyncService : IRoomStatusSyncService
    {

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly RoomRepository _roomRepo;
        private readonly IEndpointProvider _endpoint;

        public RoomStatusSyncService(RoomRepository roomRepo, IEndpointProvider endpoint) {
            _roomRepo = roomRepo;
            _endpoint = endpoint;
        }

        public async Task SyncRooms()
        {

            try
            {

                IEnumerable<Room> rooms = await _roomRepo.GetAll();

                foreach (Room room in rooms) {

                    room.Occupied = _endpoint.GetPeopleCount(room.EndpointIP)>0;

                    await _roomRepo.Update(room);

                }

            }
            catch (Exception e) {
                _logger.Error(e.Demystify(), "Failed syncing room status: "+e);
                throw;
            }

        }

    }
}