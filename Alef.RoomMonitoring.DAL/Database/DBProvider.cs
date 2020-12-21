using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.DAL.Database.Interfaces;
using Dapper;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Database
{
    public class DBProvider : IDBProvider
    {

        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private SqlConnection connection;
        private string connectionString;

        public DBProvider(IConnectionStringProvider connectionStringProvider) {

            this.connectionString = connectionStringProvider.Value;

        }

        private SqlConnection getConnection() {

            if (connection == null) {

                try
                {
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                }
                catch (Exception e)
                {
                    _logger.Error(e.Demystify(), "Failed connecting to database (Connection string="+connectionString+")");
                    throw;
                }

            }

            return connection;

        }

        public async Task<int> ExecuteAsync(string sql)
        {
            try
            {
                return await getConnection().ExecuteAsync(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed executing '" + sql + "'");
                throw;
            }
        }

        public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string sql)
        {
            try
            {
                return await getConnection().QueryAsync<T>(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying '" + sql + "'");
                throw;
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql)
        {
            try
            {
                return await getConnection().ExecuteScalarAsync<T>(sql);
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed executing scalar '" + sql + "'");
                throw;
            }
        }

        public void Close() {

            if (connection != null) {
                try
                {
                    connection.Close();
                }
                catch (Exception e)
                {
                    _logger.Error(e.Demystify(), "Failed closing connection");
                    throw;
                }
            }

        }

    }
}
