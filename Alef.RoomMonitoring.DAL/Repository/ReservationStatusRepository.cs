using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.DAL.Database.Interfaces;
using Alef.RoomMonitoring.DAL.Model;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Repository
{
    public class ReservationStatusRepository : BaseRepository, IReservationStatusRepository
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ReservationStatusRepository(IDBProvider database) : base(database)
        {
        }

        public async Task<IEnumerable<ReservationStatus>> GetAll()
        {
            try
            {
                return await Database.ExecuteQueryAsync<ReservationStatus>("select * from ReservationStatus");
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table ReservationStatus");
                throw;
            }
        }

        public async Task<ReservationStatus> GetByName(string name)
        {
            try
            {
                string sql = "select * from ReservationStatus where Name='" + name + "'";
                IEnumerator<ReservationStatus> query = (await Database.ExecuteQueryAsync<ReservationStatus>(sql)).GetEnumerator();
                if (!query.MoveNext())
                    return null;
                return query.Current;
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table ReservationStatus");
                throw;
            }
        }

        public async Task<ReservationStatus> GetById(int id)
        {
            try
            {
                string sql = "select * from ReservationStatus where Id='" + id + "'";
                IEnumerator<ReservationStatus> query = (await Database.ExecuteQueryAsync<ReservationStatus>(sql)).GetEnumerator();
                if (!query.MoveNext())
                    return null;
                return query.Current;
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table ReservationStatus");
                throw;
            }
        }

    }
}
