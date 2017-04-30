using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.EntityFrameworkCore
{
    public class EFStudentsRepository: EFGenericRepository<Student>, IStudentsRepository
    {        
        public EFStudentsRepository(StudentsDbContext dbContext):base(dbContext)
        {

           /* if (_dbContext.Database.EnsureCreated())
            {
                //string format = "yyyy-MM-ddTHH:MM";
                //database was created we need to add some info here
                _dbSet.Add(new Student() { FirstName = "John", LastName = "Doe", BirthDay = DateTime.UtcNow });
                _dbSet.Add(new Student() { FirstName = "Jean", LastName = "Valjean", BirthDay = Convert.ToDateTime("2001-05-01T07:54:59.9843750-04:00") });
                _dbSet.Add(new Student() { FirstName = "Jack", LastName = "Chirac", BirthDay = Convert.ToDateTime("1999-05-01T07:54:59.9843750-04:00") });
                _dbSet.Add(new Student() { FirstName = "Diana", LastName = "Krall", BirthDay = Convert.ToDateTime("1990-05-01T07:54:59.9843750-04:00") });

            }
            _dbContext.SaveChanges();*/
        }
        public IEnumerable<Student> GetByLastName(string lastName)
        {
            return _dbSet.Where(student => student.LastName.Contains(lastName)).ToList();            
        }        
    }
}
