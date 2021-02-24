using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.DAL.Database.Interfaces;
using Alef.RoomMonitoring.DAL.Database.WhereConstraints;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Dapper;
using NLog;

namespace Alef.RoomMonitoring.DAL.Repository
{
    /// <summary>
    /// Base class for all repositories
    /// </summary>
    public abstract class BaseRepository<T> : IBaseRepository<T>
    {

        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public IDBProvider Database { get; }
        public string TableName { get; set; }
        public string IdentityField { get; set; }
        public string[] Fields { get; set; }

        public BaseRepository(IDBProvider database) {
            Database = database;
        }

        public R Create<R>(T t) {

            try
            {

                string sql = "insert into " + TableName + "(";
                for (int i = 0; i < Fields.Length; i++) {
                    sql += Fields[i];
                    if (i < Fields.Length - 1) {
                        sql += ",";
                    }
                }
                sql += ") output inserted."+IdentityField+" values (";
                for (int i = 0; i < Fields.Length; i++)
                {
                    sql += "@"+Fields[i];
                    if (i < Fields.Length - 1)
                    {
                        sql += ",";
                    }
                }
                sql += ");";

                return Database.ExecuteScalar<R>(sql, t);

            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed inserting into table "+TableName);
                throw;
            }

        }

        public void Update(T t) {
            try
            {

                string sql = "update " + TableName + " set ";
                for (int i = 0; i < Fields.Length; i++)
                {
                    sql += Fields[i]+"=@" + Fields[i];
                    if (i < Fields.Length - 1)
                    {
                        sql += ",";
                    }
                }
                sql += " where "+IdentityField+"=@"+IdentityField+";";

                Database.Execute(sql, t);

            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed updating table "+TableName);
                throw;
            }
        }

        public void Delete(T t) {

            try
            {
                string sql = "delete from " + TableName + " where " + IdentityField + "=@" + IdentityField + ";";
                Database.Execute(sql, t);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed deleting from table"+TableName);
                throw;
            }

        }

        public IEnumerable<T> GetAll() {

            try
            {
                string sql = "select * from " + TableName + ";";
                return Database.ExecuteQuery<T>(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed querying from table" + TableName);
                throw;
            }

        }

        public IEnumerable<T> GetWhere(IConstraint constraint) {

            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                string constraintSql = constraint.Build(parameters);

                string sql = "select * from " + TableName + " where " + constraintSql;

                return Database.ExecuteQuery<T>(sql, new DynamicParameters(parameters));

            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed querying from table" + TableName);
                throw;
            }

        }

        public T GetByProperty(string name, object value) {

            try
            {

                string sql = "select * from " + TableName + " where " + name + "=@" + name + ";";

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add(name, value);

                IEnumerable<T> res = Database.ExecuteQuery<T>(sql, parameters);
                return res.FirstOrDefault();

            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed querying from table" + TableName);
                throw;
            }

        }

        public void DeleteWhere(IConstraint constraint) {

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                string constraintSql = constraint.Build(parameters);

                string sql = "delete from " + TableName + " where " + constraintSql;

                Database.ExecuteQuery<T>(sql, new DynamicParameters(parameters));
            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed deleting from table" + TableName);
                throw;
            }

        }

    }
}
