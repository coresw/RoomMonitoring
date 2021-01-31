using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.DAL.Database.Interfaces;
using Alef.RoomMonitoring.DAL.Database.WhereConstraints;
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

        public void Create(Room r)
        {
            try
            {
                string sql = "insert into Room(Name, EMail, EndpointIP) values (" +
                    "'" + r.Name + "'" +
                    ",'" + r.EMail + "'" +
                    ",'" + r.EndpointIP + "'" +
                    ")";
                Database.Execute(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed inserting into table Room");
                throw;
            }
        }

        public void Delete(Room r)
        {
            try
            {
                string sql = "delete from Room where " +
                    "EMail='" + r.EMail + "'";
                Database.Execute(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed deleting from table Room");
                throw;
            }
        }

        public IEnumerable<Room> GetAll()
        {
            try
            {
                return Database.ExecuteQuery<Room>("select * from Room");
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table Room");
                throw;
            }
        }

        public IEnumerable<Room> GetWhere(IConstraint constraint)
        {
            try
            {
                string sql = "select * from Room where " + constraint.BuildSQL();
                return Database.ExecuteQuery<Room>(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table Room");
                throw;
            }
        }

        public Room GetByEMail(string email)
        {
            try
            {
                string sql = "select * from Room where " +
                    "EMail='" + email + "'";
                IEnumerator<Room> query = Database.ExecuteQuery<Room>(sql).GetEnumerator();
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

        public Room GetById(int id)
        {
            try
            {
                string sql = "select * from Room where Id='" + id + "'";
                IEnumerator<Room> query = Database.ExecuteQuery<Room>(sql).GetEnumerator();
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

        public void Update(Room r)
        {
            try
            {
                string sql = "update Room set " +
                    "Name='" + r.Name + "'" +
                    ", EndpointIP='" + r.EndpointIP + "'" +
                    " where EMail='" + r.EMail + "'";
                Database.Execute(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed updating table Room");
                throw;
            }
        }

        public void DeleteWhere(IConstraint constraint)
        {
            try
            {
                string sql = "delete from Room where " + constraint.BuildSQL();
                Database.Execute(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed deleting from table Room");
                throw;
            }
        }
    }
}
