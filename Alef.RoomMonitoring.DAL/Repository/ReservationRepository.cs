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
    public class ReservationRepository : BaseRepository, IReservationRepository
    {

        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private static readonly DateTime _def_date = new DateTime();
        private readonly DbConfiguration _config;

        public ReservationRepository(IDBProvider database, IConfigFileBootstrapLoader config) : base(database)
        {
            _config = config.GetDbConfiguration();
        }

        public void Create(Reservation r)
        {
            try
            {

                string sql = "insert into Reservation(Token, Created, Modified, TimeFrom, TimeTo, Name, Body, ReservationStatusID, RoomID) values (" +
                    "'" + r.Token + "'" +
                    ",'" + (r.Created==_def_date ? null : r.Created.ToString(_config.DateFormat)) + "'" +
                    ",'" + (r.Modified == _def_date ? null : r.Modified.ToString(_config.DateFormat)) + "'" +
                    ",'" + (r.TimeFrom == _def_date ? null : r.TimeFrom.ToString(_config.DateFormat)) + "'" +
                    ",'" + (r.TimeTo == _def_date ? null : r.TimeTo.ToString(_config.DateFormat)) + "'" +
                    ",'" + r.Name + "'" +
                    ",'" + r.Body + "'" +
                    ",'" + r.ReservationStatusId + "'" +
                    ",'" + r.RoomId + "'" +
                    ")";
                Database.Execute(sql);
                r.Id = GetByToken(r.Token).Id;
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed inserting into table Reservation");
                throw;
            }
        }

        public void Delete(Reservation r)
        {
            try
            {
                string sql = "delete from Reservation where Token='" + r.Token + "'";
                Database.Execute(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed deleting from table Reservation");
                throw;
            }
        }

        public IEnumerable<Reservation> GetAll()
        {
            try
            {
                return Database.ExecuteQuery<Reservation>("select * from Reservation");
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table Reservation");
                throw;
            }
        }

        public Reservation GetByToken(string token)
        {
            try
            {
                string sql = "select * from Reservation where Token='" + token + "'";
                IEnumerator<Reservation> query = Database.ExecuteQuery<Reservation>(sql).GetEnumerator();
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

        public IEnumerable<Reservation> GetWhere(IConstraint constraint)
        {
            try
            {

                string sql = "select * from Reservation where " + constraint.BuildSQL();
                return Database.ExecuteQuery<Reservation>(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table Reservation");
                throw;
            }
        }

        public Reservation GetById(int id)
        {
            try
            {
                string sql = "select * from Reservation where Id='" + id + "'";
                IEnumerator<Reservation> query = Database.ExecuteQuery<Reservation>(sql).GetEnumerator();
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

        public void Update(Reservation r)
        {
            try
            {
                string sql = "update Reservation set " +
                    "Created='" + (r.Created == _def_date ? null : r.Created.ToString(_config.DateFormat)) + "'" +
                    ",Modified='" + (r.Modified == _def_date ? null : r.Modified.ToString(_config.DateFormat)) + "'" +
                    ",TimeFrom='" + (r.TimeFrom == _def_date ? null : r.TimeFrom.ToString(_config.DateFormat)) + "'" +
                    ",TimeTo='" + (r.TimeTo == _def_date ? null : r.TimeTo.ToString(_config.DateFormat)) + "'" +
                    ",Name='" + r.Name + "'" +
                    ",Body='" + r.Body + "'" +
                    ",ReservationStatusId='" + r.ReservationStatusId + "'" +
                    ",RoomId='" + r.RoomId + "'" +
                    " where Token='" + r.Token + "'" +
                    "";
                Database.Execute(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed updating table Reservation");
                throw;
            }
        }

        public void DeleteWhere(IConstraint constraint)
        {
            try
            {
                string sql = "delete from Reservation where " + constraint.BuildSQL();
                Database.Execute(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed deleting from table Reservation");
                throw;
            }
        }
    }
}
