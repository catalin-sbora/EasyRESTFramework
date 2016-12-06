﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EasyRESTTestServer.Models;
using EasyRESTTestServer.DataAccess;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EasyRESTTestServer.Controllers
{
    
    [Route("api/[controller]")]
    public class StudentsController : Controller
    {
        private StudentsDbContext dbContext = new StudentsDbContext();
        // GET: api/values
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            return dbContext.Students.ToList();//new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
