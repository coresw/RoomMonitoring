using Alef.RoomMonitoring.DAL.Database.Interfaces;
using Alef.RoomMonitoring.DAL.Database.WhereConstraints;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Repository.Interfaces
{
    public interface IBaseRepository<T>
    {

        IDBProvider Database { get; }
        string TableName { get; set; }
        string IdentityField { get; set; }
        string[] Fields { get; set; }

        public R Create<R>(T t);

        public void Update(T t);

        public void Delete(T t);

        public IEnumerable<T> GetAll();

        public IEnumerable<T> GetWhere(IConstraint constrint);

        public T GetByProperty(string name, object value);

        public void DeleteWhere(IConstraint constraint);

    }
}
