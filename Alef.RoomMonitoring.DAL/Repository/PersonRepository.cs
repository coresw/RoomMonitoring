using System;
using NLog;
using Alef.RoomMonitoring.DAL.Model;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Alef.RoomMonitoring.Configuration.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Alef.RoomMonitoring.DAL.Database.Interfaces;
using System.Diagnostics;
using Alef.RoomMonitoring.DAL.Database.WhereConstraints;

namespace Alef.RoomMonitoring.DAL.Repository
{
    /// <summary>
    /// Implements repository for entity Person
    /// </summary>
    public class PersonRepository : BaseRepository, IPersonRepository
    {

        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public PersonRepository(IDBProvider database) : base(database)
        {
        }

        public void Create(Person p)
        {
            try
            {
                string sql = "insert into Person(Name, EMail) values (" +
                    "'" + p.Name + "', " +
                    "'" + p.EMail + "'" +
                    ")";
                Database.Execute(sql);
                p.Id = GetByEMail(p.EMail).Id;
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed inserting into table Person");
                throw;
            }
        }

        public void Delete(Person p)
        {
            try
            {
                string sql = "delete from Person where EMail='"+p.EMail+"'";
                Database.Execute(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed deleting from table Person");
                throw;
            }
        }

        public IEnumerable<Person> GetAll()
        {
            try
            {
                return Database.ExecuteQuery<Person>("select * from Person");
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table Person");
                throw;
            }
        }

        public IEnumerable<Person> GetWhere(IConstraint constraint)
        {
            try
            {
                string sql = "select * from Person where " + constraint.BuildSQL();
                return Database.ExecuteQuery<Person>(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table Person");
                throw;
            }
        }

        public Person GetByEMail(string eMail)
        {
            try
            {
                string sql = "select * from Person where EMail='" + eMail + "'";
                IEnumerator<Person> query = Database.ExecuteQuery<Person>(sql).GetEnumerator();
                if (!query.MoveNext())
                    return null;
                return query.Current;
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table Person");
                throw;
            }
        }

        public Person GetById(int id)
        {
            try
            {
                string sql = "select * from Person where Id='" + id + "'";
                IEnumerator<Person> query = Database.ExecuteQuery<Person>(sql).GetEnumerator();
                if (!query.MoveNext())
                    return null;
                return query.Current;
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table Person");
                throw;
            }
        }

        public void Update(Person p)
        {
            try
            {
                string sql = "update Person set " +
                    "Name='" + p.Name + "'" +
                    " where EMail='"+p.EMail+"'" +
                    "";
                Database.Execute(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed updating table Person");
                throw;
            }
        }

        public void DeleteWhere(IConstraint constraint)
        {
            try
            {
                string sql = "delete from Person where " + constraint.BuildSQL();
                Database.Execute(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed deleting from table Person");
                throw;
            }
        }
    }
}
