using System;
using NLog;
using Alef.RoomMonitoring.DAL.Model;
using Alef.RoomMonitoring.DAL.Repository.Interfaces;
using Alef.RoomMonitoring.Configuration.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Alef.RoomMonitoring.DAL.Database.Interfaces;
using System.Diagnostics;
using Alef.RoomMonitoring.DAL.Database.WhereConstraints;

namespace Alef.RoomMonitoring.DAL.Repository
{
    /// <summary>
    /// Implements repository for entity Person
    /// </summary>
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {

        public PersonRepository(IDBProvider database) : base(database)
        {

            TableName = "Person";
            IdentityField = Person.ID;
            Fields = new string[] { 
                Person.NAME, Person.EMAIL
            };

        }

        public void Create(Person p)
        {
            p.Id = base.Create<int>(p);
        }

    }
}
