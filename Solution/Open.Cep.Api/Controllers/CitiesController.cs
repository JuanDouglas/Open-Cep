using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Open.Cep.Api.Controllers
{
   [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        [Route("ByID")]
        [HttpGet]
        public async Task<ActionResult> ByNumber(int id)
        {
            City city = Program.Load.Cities.FirstOrDefault(fs => fs.ID==id);
            if (city == null)
                return NotFound();
            return Ok(city);
        }

       
    }
}