using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Interfaces;
using DataAccess.Models;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EasyRESTServer.Controllers
{
    
    [Route("api/[controller]")]
    public class StudentsController : Controller
    {
        private readonly IPersistenceContext _persistContext;
        private readonly IStudentsRepository _studentsRepo;
        public StudentsController(IPersistenceContext persistenceContext)
        {
            _persistContext = persistenceContext;
            _studentsRepo = _persistContext.GetStudentsRepository();
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            return _studentsRepo.GetAll();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Student Get(int id)
        {
            return _studentsRepo.GetById(id);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]IEnumerable<Student> students)
        {
            foreach(Student s in students)
            {
                if (s.Id < 0)
                    s.Id = 0;

                _studentsRepo.Add(s);
            }
            _persistContext.SaveAll();
            //_persistContext.
            JsonResult result = Json(students);
            return result;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Student value)
        {
            //update
           bool updated = _studentsRepo.Update(value);
            _persistContext.SaveAll();
            return Json(value);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Student studentToDelete = _studentsRepo.GetById(id);
            if (studentToDelete != null)
            {
                _studentsRepo.Remove(studentToDelete);
            }

            return new EmptyResult();
        }
    }
}
