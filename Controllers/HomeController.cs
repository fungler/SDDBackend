using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SDDBackend.Models;
using Newtonsoft.Json;

namespace SDDBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET: api/<HomeController>
        [HttpGet]
        public String Get()
        {
            return "Running";
        }

        [HttpPost("registerJson")]
        public async Task<IActionResult> Post([FromBody] InstallationRoot payload)
        {
            // extract info
            //return Ok("Recieved: " + payload.installation.tags.costcenter);

            //return payload as json string
            //return Ok(JsonConvert.SerializeObject(payload));

            var jsonString = JsonConvert.SerializeObject(payload, Formatting.Indented);

            await GitController.createFile("Testing JSON 2", jsonString, "./installation/some_installation2");

            return Ok("My name sure do be jeff");
        }

    }
}
