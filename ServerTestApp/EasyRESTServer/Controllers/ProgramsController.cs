using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Models;
namespace EasyRESTServer.Controllers
{
    [Route("api/[controller]")]
    public class ProgramsController:Controller
    {
        private readonly IPersistenceContext _persistContext;
        private readonly IProgramsRepository _programsRepo;
        public ProgramsController(IPersistenceContext persistenceContext)
        {
            _persistContext = persistenceContext;
            _programsRepo = _persistContext.GetProgramsRepository();
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<DataAccess.Models.Program> Get()
        {
            var allPrograms = _programsRepo.GetAll();
            var days = allPrograms.ElementAt(0).WateringDays;
            return allPrograms;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public DataAccess.Models.Program Get(int id)
        {
            return _programsRepo.GetById(id);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]IEnumerable<DataAccess.Models.Program> programs)
        {
            foreach (DataAccess.Models.Program s in programs)
            {
                if (s.Id < 0)
                    s.Id = 0;

                _programsRepo.Add(s);
            }
            _persistContext.SaveAll();
            //_persistContext.
            JsonResult result = Json(programs);
            return result;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]DataAccess.Models.Program value)
        {
            //update
            bool updated = _programsRepo.Update(value);
            _persistContext.SaveAll();
            return Json(value);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var programToDelete = _programsRepo.GetById(id);
            if (programToDelete != null)
            {
                _programsRepo.Remove(programToDelete);
            }

            return new EmptyResult();
        }
    }
}
