using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalRandkowy.API.Data;
using PortalRandkowy.API.Models;

namespace PortalRandkowy.API.Controllers
{
    //http://localhost:5000/api/Values
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext context;
        public ValuesController(DataContext context)
        {
            this.context = context;
        }
        // GET api/values
        [HttpGet]
        public IActionResult GetValues()
        {
            var values = this.context.Values.ToList();
            return Ok(values);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult GetValue(int id)
        {
            var value = this.context.Values.FirstOrDefault(x=>x.Id == id);
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public IActionResult AddValue([FromBody] Value value)
        {
            this.context.Values.Add(value);
            this.context.SaveChanges();
            return Ok(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult EditValue(int id, [FromBody] Value value)
        {
            var data = this.context.Values.Find(id);
            data.Name = value.Name;
            this.context.Values.Update(data);
            this.context.SaveChanges();
            return Ok(data);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult DeleteValue(int id)
        {
            var data = this.context.Values.Find(id);
            this.context.Values.Remove(data);
            this.context.SaveChanges();
            return Ok(data);
        }
    }
}
