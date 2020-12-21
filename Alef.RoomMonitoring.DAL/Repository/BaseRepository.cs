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
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Dapper;
using NLog;

namespace Alef.RoomMonitoring.DAL.Repository
{
    /// <summary>
    /// Base class for all repositories
    /// </summary>
    public abstract class BaseRepository : IBaseRepository
    {

        public IDBProvider Database { get; }

        public BaseRepository(IDBProvider database) {
            Database = database;
        }

    }
}
