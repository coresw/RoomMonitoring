﻿using Alef.RoomMonitoring.Configuration.ConfigFileSections;
using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.DAL.Database.Interfaces;
using Alef.RoomMonitoring.DAL.Database.WhereConstraints;
using Alef.RoomMonitoring.DAL.Model;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Repository
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {

        public ReservationRepository(IDBProvider database) : base(database)
        {

            TableName = "Reservation";
            IdentityField = Reservation.ID;
            Fields = new string[] { 
                Reservation.TOKEN, Reservation.CREATED, Reservation.MODIFIED, Reservation.NAME, Reservation.BODY,
                Reservation.TIME_FROM, Reservation.TIME_TO, Reservation.ROOM_ID, Reservation.RESERVATION_STATUS_ID
            };

        }

        public void Create(Reservation r)
        {
            r.Id = base.Create<int>(r);
        }

    }
}
