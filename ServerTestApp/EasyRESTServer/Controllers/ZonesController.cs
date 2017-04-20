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
    public class ZonesController : Controller
    {
        private readonly IPersistenceContext _persistContext;
        private readonly IZonesRepository _zonesRepo;
        public ZonesController(IPersistenceContext persistenceContext)
        {
            _persistContext = persistenceContext;
            _zonesRepo = _persistContext.GetZonesRepository();
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Zone> Get()
        {
            return _zonesRepo.GetAll();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Zone Get(int id)
        {
            return _zonesRepo.GetById(id);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]IEnumerable<Zone> zones)
        {
            foreach (Zone s in zones)
            {
                if (s.Id < 0)
                    s.Id = 0;

                _zonesRepo.Add(s);
            }
            _persistContext.SaveAll();
            //_persistContext.
            JsonResult result = Json(zones);
            return result;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Zone value)
        {
            //update
            bool updated = _zonesRepo.Update(value);
            _persistContext.SaveAll();
            return Json(value);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var zoneToDelete = _zonesRepo.GetById(id);
            if (zoneToDelete != null)
            {
                _zonesRepo.Remove(zoneToDelete);
            }

            return new EmptyResult();
        }
    }
}

