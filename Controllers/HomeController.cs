using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SDDBackend.Models;
using Newtonsoft.Json;
using Octokit;
using Microsoft.AspNetCore.Cors;

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
        public async Task<IActionResult> postJson([FromBody] InstallationRoot payload)
        {  
            try
            {
                var jsonString = JsonConvert.SerializeObject(payload, Formatting.Indented);
                /*foreach (Vm vm in payload.installation.vms)
                {
                    await GitController.createFile("Create: " + payload.installation.name, jsonString, "./installations/" + payload.installation.name + "/" + vm.name + ".json");
                }*/
                await GitController.createFile("Create: " + payload.installation.name, jsonString, "./installations/" + payload.installation.name + "/" + payload.installation.name + ".json");
            }
            catch (ApiValidationException e)
            {
                return BadRequest("{\"status\": 400, \"message\": \"File already exists in github repo.\"}");
            }
            catch (Exception e)
            {
                return BadRequest("{\"status\": 400, \"message\": \"Unknown error.\"}");
            }

            return Ok("{\"status\": 200, \"message\": \"Success.\"}");
        }

        [HttpPost("registerJson/copy")]
        public async Task<IActionResult> copyJson([FromBody] CopyData data)
        {
            try
            {    
                IActionResult actionResult = await getJson("installations/" + data.oldName + "/" + data.oldName + ".json");
                OkObjectResult content = (OkObjectResult)actionResult;

                string jsonString = (string)content.Value;
                jsonString = jsonString.Replace(data.oldName, data.newName);

                await GitController.createFile("Copy: " + data.oldName + " as " + data.newName, jsonString, "./installations/" + data.newName + "/" + data.newName + ".json");
            
            }
            catch (NullReferenceException e)
            {
                return BadRequest("{\"status\": 400, \"message\": \"Could not find file with the given filename.\"}");
            }
            catch (ApiValidationException e)
            {
                return BadRequest("{\"status\": 400, \"message\": \"File already exists in github repo.\"}");
            }
            catch (Exception e)
            {
                return BadRequest("{\"status\": 400, \"message\": \"Unknown error.\"}");
            }

            return Ok("{\"status\": 200, \"message\": \"Success.\"}");
        }

        [HttpGet("registerJson/getFile")]
        public async Task<IActionResult> getJson([FromQuery] string path)
        {
            try
            {
                var content = await GitController.getFile(path);
                var res = content.ElementAt<RepositoryContent>(0);

                return Ok(res.Content);
            }
            catch (Exception e)
            {
                return BadRequest("Error getting file.");
            }
        }

        [HttpGet("registerJson/getState")]
        public async Task<IActionResult> getState([FromQuery] string name)
        {
            string path = "installations/" + name + "/" + name + ".json";

            try
            {
                var content = await GitController.getFile(path);
                var res = content.ElementAt<RepositoryContent>(0);

                InstallationRoot installation = JsonConvert.DeserializeObject<InstallationRoot>(res.Content);

                string state = installation.installation.state;

                return Ok(state);
            }
            catch (Exception e)
            {
                return BadRequest("Error getting state.");
            }
        }

    }
}
