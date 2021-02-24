using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Database.Interfaces
{
    public interface IDBProvider
    {

        int Execute(string sql, object parameters = null);
        IEnumerable<T> ExecuteQuery<T>(string sql, object parameters = null);
        T ExecuteScalar<T>(string sql, object parameters = null);
        void Close();

    }
}
