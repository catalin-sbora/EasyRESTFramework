using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DataModels;

namespace DataAccess.Interfaces
{
    public interface IStudentsRepository : IGenericRepository<Student>
    {
        IEnumerable<Student> GetByLastName(string lastName);
    }
}
