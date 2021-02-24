using Alef.RoomMonitoring.DAL.Database.Interfaces;
using Alef.RoomMonitoring.DAL.Model;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.DAL.Repository
{
    public class AttendeeTypeRepository: BaseRepository<AttendeeType>, IAttendeeTypeRepository
    {

        public AttendeeTypeRepository(IDBProvider database) : base(database) {

            TableName = "AttendeeType";
            IdentityField = AttendeeType.ID;
            Fields = new string[] {
                AttendeeType.NAME
            };

        }

        public void Create(AttendeeType a) { }

    }
}
