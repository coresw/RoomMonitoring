using Alef.RoomMonitoring.DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alef.RoomMonitoring.DAL.Repository.Interfaces
{
    public interface IPersonRepository 
    {
        Task Create(Person p);
        Task<Person> GetById(int id);
        Task<Person> GetByEMail(string eMail);
        Task<IEnumerable<Person>> GetAll();
        Task Update(Person p);
        Task Delete(Person p);
    }
}
