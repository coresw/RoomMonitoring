using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.DAL.Database.Interfaces;
using Alef.RoomMonitoring.DAL.Database.WhereConstraints;
using Alef.RoomMonitoring.DAL.Model;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Repository
{
    public class AttendeeRepository: BaseRepository, IAttendeeRepository
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public AttendeeRepository(IDBProvider database) : base(database)
        {
        }

        public void Create(Attendee a)
        {
            try
            {
                string sql = "insert into Attendee(PersonId, ReservationId, AttendeeTypeId) values (" +
                    "'" + a.PersonId + "'" +
                    ",'" + a.ReservationId + "'" +
                    ",'" + a.AttendeeTypeId + "'" +
                    ")";
                Database.Execute(sql);
                a.Id = GetByPersonReservation(a.PersonId, a.ReservationId).Id;
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed inserting into table Attendee");
                throw;
            }
        }

        public void Delete(Attendee a)
        {
            try
            {
                string sql = "delete from Attendee where " +
                    "PersonId='" + a.PersonId + "' and "+
                    "ReservationId='" + a.ReservationId + "'";
                Database.Execute(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed deleting from table Attendee");
                throw;
            }
        }

        public IEnumerable<Attendee> GetAll()
        {
            try
            {
                return Database.ExecuteQuery<Attendee>("select * from Attendee");
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table Attendee");
                throw;
            }
        }

        public Attendee GetByPersonReservation(int personId, int reservationId)
        {
            try
            {
                string sql = "select * from Attendee where " +
                    "PersonId='" + personId + "' and " +
                    "ReservationId='" + reservationId + "'";
                IEnumerator<Attendee> query = Database.ExecuteQuery<Attendee>(sql).GetEnumerator();
                if (!query.MoveNext())
                    return null;
                return query.Current;
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table Attendee");
                throw;
            }
        }

        public Attendee GetById(int id)
        {
            try
            {
                string sql = "select * from Attendee where Id='" + id + "'";
                IEnumerator<Attendee> query = Database.ExecuteQuery<Attendee>(sql).GetEnumerator();
                if (!query.MoveNext())
                    return null;
                return query.Current;
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table Attendee");
                throw;
            }
        }

        public IEnumerable<Attendee> GetWhere(IConstraint constraint)
        {
            try
            {
                string sql = "select * from Attendee where "+constraint.BuildSQL();
                return Database.ExecuteQuery<Attendee>(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table Attendee");
                throw;
            }
        }

        public void Update(Attendee a)
        {
            try
            {
                string sql = "update Attendee set " +
                    "AttendeeTypeId='" + a.AttendeeTypeId + "'" +
                    " where PersonId='" + a.PersonId + "' and" +
                    " ReservationId='" + a.ReservationId + "'" +
                    "";
                Database.Execute(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed updating table Attendee");
                throw;
            }
        }

        public void DeleteWhere(IConstraint constraint)
        {
            try
            {
                string sql = "delete from Attendee where " + constraint.BuildSQL();
                Database.Execute(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed deleting table Attendee");
                throw;
            }
        }
    }
}
