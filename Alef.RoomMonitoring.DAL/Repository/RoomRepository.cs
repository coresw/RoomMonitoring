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
    public class RoomRepository : BaseRepository, IRoomRepository
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public RoomRepository(IDBProvider database) : base(database)
        {
        }

        public async Task Create(Room r)
        {
            try
            {
                string sql = "insert into Room(Name, Occupied) values (" +
                    "'" + r.Name + "'" +
                    ",'" + r.Occupied + "'" +
                    ")";
                await Database.ExecuteAsync(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed inserting into table Room");
                throw;
            }
        }

        public async Task Delete(Room r)
        {
            try
            {
                string sql = "delete from Room where " +
                    "Name='" + r.Name + "'";
                await Database.ExecuteAsync(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed deleting from table Room");
                throw;
            }
        }

        public async Task<IEnumerable<Room>> GetAll()
        {
            try
            {
                return await Database.ExecuteQueryAsync<Room>("select * from Room");
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table Room");
                throw;
            }
        }

        public async Task<Room> GetByName(string name)
        {
            try
            {
                string sql = "select * from Room where " +
                    "Name='" + name + "'";
                IEnumerator<Room> query = (await Database.ExecuteQueryAsync<Room>(sql)).GetEnumerator();
                if (!query.MoveNext())
                    return null;
                return query.Current;
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table Room");
                throw;
            }
        }

        public async Task<Room> GetById(int id)
        {
            try
            {
                string sql = "select * from Room where Id='" + id + "'";
                IEnumerator<Room> query = (await Database.ExecuteQueryAsync<Room>(sql)).GetEnumerator();
                if (!query.MoveNext())
                    return null;
                return query.Current;
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table Room");
                throw;
            }
        }

        public async Task Update(Room r)
        {
            try
            {
                string sql = "update Room set " +
                    "Occupied='" + r.Occupied + "'" +
                    " where Name='" + r.Name + "'";
                await Database.ExecuteAsync(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed updating table Room");
                throw;
            }
        }

    }
}
