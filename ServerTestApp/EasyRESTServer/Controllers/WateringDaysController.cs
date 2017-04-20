using DataAccess.DataModels;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRESTServer.Controllers
{
    [Route("api/[controller]")]
    public class WateringDaysController:Controller
    {
        private readonly IPersistenceContext _persistContext;
        private readonly IWateringDaysRepository _wateringDaysRepo;
        public WateringDaysController(IPersistenceContext persistenceContext)
        {
            _persistContext = persistenceContext;
            _wateringDaysRepo = _persistContext.GetWateringDaysRepository();
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<WateringDay> Get()
        {
            return _wateringDaysRepo.GetAll();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public WateringDay Get(int id)
        {
            return _wateringDaysRepo.GetById(id);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]IEnumerable<WateringDay> days)
        {
            foreach (WateringDay s in days)
            {
                if (s.Id < 0)
                    s.Id = 0;

                _wateringDaysRepo.Add(s);
            }
            _persistContext.SaveAll();
            //_persistContext.
            JsonResult result = Json(days);
            return result;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]WateringDay value)
        {
            //update
            bool updated = _wateringDaysRepo.Update(value);
            _persistContext.SaveAll();
            return Json(value);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var dayToDelete = _wateringDaysRepo.GetById(id);
            if (dayToDelete != null)
            {
                _wateringDaysRepo.Remove(dayToDelete);
            }

            return new EmptyResult();
        }
    }
}
