using System;
using NLog;
using Alef.RoomMonitoring.DAL.Model;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Alef.RoomMonitoring.Configuration.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Alef.RoomMonitoring.DAL.Database.Interfaces;
using System.Diagnostics;

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

        public async Task Create(Person p)
        {
            try
            {
                string sql = "insert into Person(Name, EMail) values (" +
                    "'" + p.Name + "', " +
                    "'" + p.EMail + "'" +
                    ")";
                await Database.ExecuteAsync(sql);
                p.Id = (await GetByEMail(p.EMail)).Id;
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed inserting into table Person");
                throw;
            }
        }

        public async Task Delete(Person p)
        {
            try
            {
                string sql = "delete from Person where EMail='"+p.EMail+"'";
                await Database.ExecuteAsync(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed deleting from table Person");
                throw;
            }
        }

        public async Task<IEnumerable<Person>> GetAll()
        {
            try
            {
                return await Database.ExecuteQueryAsync<Person>("select * from Person");
            }
            catch(Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying from table Person");
                throw;
            }
        }

        public async Task<Person> GetByEMail(string eMail)
        {
            try
            {
                string sql = "select * from Person where EMail='" + eMail + "'";
                IEnumerator<Person> query = (await Database.ExecuteQueryAsync<Person>(sql)).GetEnumerator();
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

        public async Task<Person> GetById(int id)
        {
            try
            {
                string sql = "select * from Person where Id='" + id + "'";
                IEnumerator<Person> query = (await Database.ExecuteQueryAsync<Person>(sql)).GetEnumerator();
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

        public async Task Update(Person p)
        {
            try
            {
                string sql = "update Person set " +
                    "Name='" + p.Name + "'" +
                    " where EMail='"+p.EMail+"'" +
                    "";
                await Database.ExecuteAsync(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed updating table Person");
                throw;
            }
        }
    }
}
