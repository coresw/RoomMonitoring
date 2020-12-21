using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.DAL.Database.Interfaces;
using Alef.RoomMonitoring.DAL.Model;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Repository
{
    public class AttendeeTypeRepository : BaseRepository, IAttendeeTypeRepository
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public AttendeeTypeRepository(IDBProvider database) : base(database)
        {
        }

        public async Task<IEnumerable<AttendeeType>> GetAll()
        {
            try
            {
                return await Database.ExecuteQueryAsync<AttendeeType>("select * from AttendeeType");
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table AttendeeType");
                throw;
            }
        }

        public async Task<AttendeeType> GetByName(string name)
        {
            try
            {
                string sql = "select * from AttendeeType where Name='" + name + "'";
                IEnumerator<AttendeeType> query = (await Database.ExecuteQueryAsync<AttendeeType>(sql)).GetEnumerator();
                if (!query.MoveNext())
                    return null;
                return query.Current;
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table AttendeeType");
                throw;
            }
        }

        public async Task<AttendeeType> GetById(int id)
        {
            try
            {
                string sql = "select * from AttendeeType where Id='" + id + "'";
                IEnumerator<AttendeeType> query = (await Database.ExecuteQueryAsync<AttendeeType>(sql)).GetEnumerator();
                if (!query.MoveNext())
                    return null;
                return query.Current;
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table AttendeeType");
                throw;
            }
        }

    }
}
