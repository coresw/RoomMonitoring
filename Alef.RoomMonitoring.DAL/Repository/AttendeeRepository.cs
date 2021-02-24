using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.DAL.Database.Interfaces;
using Alef.RoomMonitoring.DAL.Database.WhereConstraints;
using Alef.RoomMonitoring.DAL.Model;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Repository
{
    public class AttendeeRepository: BaseRepository<Attendee>, IAttendeeRepository
    {
        
        public AttendeeRepository(IDBProvider database) : base(database)
        {

            TableName = "Attendee";
            IdentityField = Attendee.ID;
            Fields = new string[] { 
                Attendee.PERSON_ID, Attendee.RESERVATION_ID, Attendee.ATTENDEE_TYPE_ID
            };

        }

        public void Create(Attendee a)
        {

            a.Id = base.Create<int>(a);

        }

    }
}
