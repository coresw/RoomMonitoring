using Alef.RoomMonitoring.DAL.Database.WhereConstraints;
using Alef.RoomMonitoring.DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Repository.Interfaces
{
    public interface IPersonRepository 
    {
        void Create(Person p);
        Person GetById(int id);
        Person GetByEMail(string eMail);
        IEnumerable<Person> GetAll();
        IEnumerable<Person> GetWhere(IConstraint constraint);
        void Update(Person p);
        void Delete(Person p);
        void DeleteWhere(IConstraint constraint);
    }
}
