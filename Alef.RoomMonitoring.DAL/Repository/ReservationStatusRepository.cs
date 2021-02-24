using Alef.RoomMonitoring.Configuration.ConfigFileSections;
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
    public class ReservationStatusRepository : BaseRepository<ReservationStatus>, IReservationStatusRepository
    {

        public ReservationStatusRepository(IDBProvider database) : base(database)
        {

            TableName = "ReservationStatus";
            IdentityField = ReservationStatus.ID;
            Fields = new string[] { 
                ReservationStatus.NAME, ReservationStatus.DISPLAY
            };

        }

        public void Create(ReservationStatus r) {}

    }
}
