using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyRESTTestClient.Models;

namespace EasyRESTTestClient.DataAccess
{
    public interface IStudentRepository
    {
        Task <IEnumerable<Student>> GetAll();

        Task <IEnumerable<Student>> GetStudentsWithFirstNameStartingWith(string startingWith);

        Task<IEnumerable<Student>> GetStudentsWithLastNameStartingWith(string startingWith);

        Task<Student> GetStudentById(int id);

        void AddStudent(Student studentToAdd);

        void DeleteStudent(Student studToDelete);    

    }
}
