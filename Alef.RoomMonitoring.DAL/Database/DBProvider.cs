﻿using Alef.RoomMonitoring.Configuration.Interfaces;
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

        public DBProvider(IConnectionStringProvider connectionStringProvider) {

            _connectionString = connectionStringProvider.Value;

        }

        private SqlConnection GetConnection() {

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

            return _connection;

        }

        public int Execute(string sql)
        {
            try
            {
                int res;
                var con = GetConnection();
                lock (con)
                {
                    res = con.Execute(sql);
                }
                return res;
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed executing '" + sql + "': "+e.Message);
                throw;
            }
        }

        public IEnumerable<T> ExecuteQuery<T>(string sql)
        {
            try
            {
                var con = GetConnection();
                IEnumerable<T> res;
                lock (con) {
                    res = con.Query<T>(sql);
                }
                return res;
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed querying '" + sql + "': "+e.Message);
                throw;
            }
        }

        public T ExecuteScalar<T>(string sql)
        {
            try
            {
                var con = GetConnection();
                T res;
                lock (con) {
                    res = con.ExecuteScalar<T>(sql);
                }
                return res;
            }
            catch (Exception e)
            {
                _logger.Error(e.Demystify(), "Failed executing scalar '" + sql + "': "+e.Message);
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
