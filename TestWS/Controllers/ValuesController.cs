using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestWS.Controllers
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private List<Student> _students = null;

        public ValuesController()
        {
            _students = new List<Student>() {
                new Student() {Id = 1, FirstName = "First Name 1", LastName = "LastNAme 1"},
                new Student() {Id = 2, FirstName = "First Name 2", LastName = "LastNAme 2"},
                new Student() { Id = 3, FirstName = "First Name 3", LastName = "LastNAme 3"}
            };
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            return _students; 
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Student Get(int id)
        {
            var ret = _students.Where(student => student.Id == id).FirstOrDefault();
           
            return ret;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
