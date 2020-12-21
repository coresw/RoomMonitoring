using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.DAL.Database.Interfaces;
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
    public class ReservationRepository : BaseRepository, IReservationRepository
    {

        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ReservationRepository(IDBProvider database) : base(database)
        {
        }

        public async Task Create(Reservation r)
        {
            try
            {
                string sql = "insert into Reservation(Token, Created, Modified, TimeFrom, TimeTo, Name, Body, ReservationStatusID, RoomID) values (" +
                    "'" + r.Token + "'" +
                    ",'" + r.Created.ToString("yyyy-MM-ddTHH:mm:ss") + "'" +
                    ",'" + r.Modified.ToString("yyyy-MM-ddTHH:mm:ss") + "'" +
                    ",'" + r.TimeFrom.ToString("yyyy-MM-ddTHH:mm:ss") + "'" +
                    ",'" + r.TimeTo.ToString("yyyy-MM-ddTHH:mm:ss") + "'" +
                    ",'" + r.Name + "'" +
                    ",'" + r.Body + "'" +
                    ",'" + r.ReservationStatusId + "'" +
                    ",'" + r.RoomId + "'" +
                    ")";
                await Database.ExecuteAsync(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed inserting into table Reservation");
                throw;
            }
        }

        public async Task Delete(Reservation r)
        {
            try
            {
                string sql = "delete from Reservation where Token='" + r.Token + "'";
                await Database.ExecuteAsync(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed deleting from table Reservation");
                throw;
            }
        }

        public async Task<IEnumerable<Reservation>> GetAll()
        {
            try
            {
                return await Database.ExecuteQueryAsync<Reservation>("select * from Reservation");
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table Reservation");
                throw;
            }
        }

        public async Task<Reservation> GetByToken(string token)
        {
            try
            {
                string sql = "select * from Reservation where Token='" + token + "'";
                IEnumerator<Reservation> query = (await Database.ExecuteQueryAsync<Reservation>(sql)).GetEnumerator();
                if (!query.MoveNext())
                    return null;
                return query.Current;
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table Reservation");
                throw;
            }
        }

        public async Task<Reservation> GetById(int id)
        {
            try
            {
                string sql = "select * from Reservation where Id='" + id + "'";
                IEnumerator<Reservation> query = (await Database.ExecuteQueryAsync<Reservation>(sql)).GetEnumerator();
                if (!query.MoveNext())
                    return null;
                return query.Current;
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table Reservation");
                throw;
            }
        }

        public async Task Update(Reservation r)
        {
            try
            {
                string sql = "update Reservation set " +
                    "Created='" + r.Created + "'" +
                    ",Modified='" + r.Modified + "'" +
                    ",TimeFrom='" + r.TimeFrom + "'" +
                    ",TimeTo='" + r.TimeTo + "'" +
                    ",Name='" + r.Name + "'" +
                    ",Body='" + r.Body + "'" +
                    ",ReservationStatusId='" + r.ReservationStatusId + "'" +
                    ",RoomId='" + r.RoomId + "'" +
                    " where Token='" + r.Token + "'" +
                    "";
                await Database.ExecuteAsync(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed updating table Reservation");
                throw;
            }
        }
    }
}
