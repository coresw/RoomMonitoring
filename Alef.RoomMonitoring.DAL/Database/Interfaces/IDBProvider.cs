using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Database.Interfaces
{
    public interface IDBProvider
    {

        Task<int> ExecuteAsync(string sql);
        Task<IEnumerable<T>> ExecuteQueryAsync<T>(string sql);
        Task<T> ExecuteScalarAsync<T>(string sql);
        void Close();

    }
}
