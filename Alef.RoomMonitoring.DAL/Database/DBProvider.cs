using Alef.RoomMonitoring.Configuration.Interfaces;
using Alef.RoomMonitoring.DAL.Database.Interfaces;
using Dapper;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Database
{
    public class DBProvider : IDBProvider
    {

        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private SqlConnection _connection;
        private string _connectionString;

        private bool _busy;

        public DBProvider(IConnectionStringProvider connectionStringProvider) {

            _connectionString = connectionStringProvider.Value;

        }

        private SqlConnection getConnection() {

            if (_connection == null) {

                try
                {
                    _connection = new SqlConnection(_connectionString);
                    _connection.Open();
                }
                catch (Exception e)
                {
                    _logger.Error(e.Demystify(), "Failed connecting to database (Connection string="+_connectionString+")");
                    throw;
                }

            }

            // TODO: replace w/ connection pool
            while (_busy)
            {
                Thread.Sleep(10); // thread safety - only one command at once
                _logger.Info("Database is busy! Request queued");
            }

            return _connection;

        }

        public async Task<int> ExecuteAsync(string sql)
        {
            try
            {
                var conn = getConnection();
                _busy = true;
                var res = await conn.ExecuteAsync(sql);
                _busy = false;
                return res;
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
                var conn = getConnection();
                _busy = true;
                var res = await conn.QueryAsync<T>(sql);
                _busy = false;
                return res;
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
                var conn = getConnection();
                _busy = true;
                var res = await conn.ExecuteScalarAsync<T>(sql);
                _busy = false;
                return res;
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed executing scalar '" + sql + "'");
                throw;
            }
        }

        public void Close() {

            if (_connection != null) {
                try
                {
                    _connection.Close();
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
