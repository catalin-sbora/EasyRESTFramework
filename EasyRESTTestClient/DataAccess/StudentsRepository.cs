using EasyRESTFramework.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyRESTTestClient.Models;
using EasyRESTFramework.Client.Abstractions;
using EasyRESTFramework.Client.Filters;

namespace EasyRESTTestClient.DataAccess
{
    public class StudentsRepository : IStudentRepository
    {
        private IWsContext _wsContext = null;
        private IWsSet<Student> _studentsSet = null;
        public StudentsRepository(IWsContext wsContext)
        {
            _wsContext = wsContext;
            _studentsSet = wsContext.Set<Student>();
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            return await _studentsSet.GetAllAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsWithFirstNameStartingWith(string startingWith)
        {
            QueryFilter filter = new QueryFilter();
            filter.AddCriteria(new QueryFilterCriteria("FirstName", CriteriaCheckCondition.StartsWith, startingWith, CriteriaType.Required));
            return await _studentsSet.GetFilteredDataAsync(filter);
        }

        public async Task<IEnumerable<Student>> GetStudentsWithLastNameStartingWith(string startingWith)
        {
            QueryFilter filter = new QueryFilter();
            filter.AddCriteria(new QueryFilterCriteria("LastName", CriteriaCheckCondition.StartsWith, startingWith, CriteriaType.Required));
            return await _studentsSet.GetFilteredDataAsync(filter);
        }

        public async Task<Student> GetStudentById(int id)
        {
            Student retVal = null;
            QueryFilter filter = new QueryFilter();
            filter.AddCriteria(new QueryFilterCriteria("Id", CriteriaCheckCondition.Equals, "" + id, CriteriaType.Required));
            var result = await _studentsSet.GetFilteredDataAsync(filter);
            if (result.Count() > 0)
            {
                retVal = result.First();
            }    
            return retVal;            
        }

        public void AddStudent(Student studentToAdd)
        {
            _studentsSet.Add(studentToAdd);
        }

        public void DeleteStudent(Student studToDelete)
        {
            throw new NotImplementedException();
        }
    }
}
