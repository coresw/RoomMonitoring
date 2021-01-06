using Alef.RoomMonitoring.Configuration;
using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.DAL.Database;
using Alef.RoomMonitoring.DAL.Database.Interfaces;
using Alef.RoomMonitoring.DAL.Model;
using Alef.RoomMonitoring.DAL.Repository;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Alef.RoomMonitoring.DAL.Services;
using Alef.RoomMonitoring.Service.Services;
using CiscoEndpointProvider;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.Service
{
    class TMP_Test
    {

        //public static void Main(string[] args)
        //{

        //    IConfigFileBootstrapLoader config = new ConfigFileBootstrapLoader();

        //    MSGraphAPI api = new MSGraphAPI(config);
        //    MSGraphProvider prov = new MSGraphProvider(api);
        //    IDBProvider db = new DBProvider(new ConnectionStringProvider(config));

        //    IReservationRepository reservRepo = new ReservationRepository(db, config);
        //    IPersonRepository personRepo = new PersonRepository(db);
        //    IAttendeeRepository attendRepo = new AttendeeRepository(db);
        //    IRoomRepository roomRepo = new RoomRepository(db);

        //    ReservationSyncService sync = new ReservationSyncService(prov, personRepo, reservRepo, attendRepo);
        //    sync.SyncReservations().Wait();

        //    RoomStatusSyncService roomSync = new RoomStatusSyncService(roomRepo, new MockEndpointProvider());
        //    roomSync.SyncRooms().Wait();

        //    CheckReservationService check = new CheckReservationService(reservRepo, roomRepo, config);
        //    check.CheckReservations().Wait();

        //    Console.WriteLine("Done!");

        //}

    }
}
