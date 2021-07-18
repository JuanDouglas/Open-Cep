using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Open.Cep.Api.Controllers
{
    [Route("api/[controller]")]
    public class CepController : ControllerBase
    {
        [Route("ByNumber")]
        [HttpGet]
        public async Task<ActionResult> ByNumber(long number)
        {
            Models.Models.Cep cep = Program.Load.Ceps.FirstOrDefault(fs => fs.Value == number);
            if (cep == null)
                return NotFound();
            return Ok(cep);
        }

        [Route("ByCity")]
        public async Task<ActionResult<Models.Models.Cep>> ByCityID(int city_id)
        {
            throw new NotImplementedException();
        }
    }
}