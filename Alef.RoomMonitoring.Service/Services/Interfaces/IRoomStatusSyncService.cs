﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.Service.Services.Interfaces
{
    interface IRoomStatusSyncService
    {
        Task SyncRooms();
    }
}