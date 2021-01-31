using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Database.Interfaces
{
    public interface IDBProvider
    {

        int Execute(string sql);
        IEnumerable<T> ExecuteQuery<T>(string sql);
        T ExecuteScalar<T>(string sql);
        void Close();

    }
}
