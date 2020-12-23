using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SDDBackend.Models;
using Newtonsoft.Json;
using Octokit;
using SDDBackend.Handlers;

namespace SDDBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        InstallationSimHandler simHandler = InstallationSimHandler.GetInstance();
        InstallationSimulation sim = new InstallationSimulation();


        [HttpPost("registerJson")]
        public async Task<IActionResult> postJson([FromBody] InstallationRoot payload, [FromQuery] string repo = "scdfiles")
        {  
            try
            {
                InstallationSim instSim = simHandler.createSuccessfulInstallation(payload, 1000, 3000);
                StatusType status = await instSim.runSetup();

                if (status == StatusType.STATUS_FINISHED_SUCCESS)
                {
                    var jsonString = JsonConvert.SerializeObject(payload, Formatting.Indented);
                    await GitController.createFile("Create: " + payload.installation.name, jsonString, "./installations/" + payload.installation.name + "/" + payload.installation.name + ".json", repo);
                }
                else
                {
                    return BadRequest("{\"status\": 400, \"message\": \"Creation of installation failed.\"}");
                }
            }
            catch (ApiValidationException e)
            {
                return BadRequest("{\"status\": 400, \"message\": \"File already exists in github repo.\"}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest("{\"status\": 400, \"message\": \"Unknown error.\"}");
            }

            return Ok("{\"status\": 200, \"message\": \"Success.\"}");
        }

        [HttpPost("registerJson/copy")]
        public async Task<IActionResult> copyJson([FromBody] CopyData data, [FromQuery] string repo = "scdfiles")
        {
            try
            {
                IActionResult actionResult = await getJson("installations/" + data.oldName + "/" + data.oldName + ".json", repo);
                var content = actionResult as OkObjectResult;

                string jsonString = content.Value.ToString();
                jsonString = jsonString.Replace(data.oldName, data.newName);

                await GitController.createFile("Copy: " + data.oldName + " as " + data.newName, jsonString, "./installations/" + data.newName + "/" + data.newName + ".json", repo);
            
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
        public async Task<IActionResult> getJson([FromQuery] string path, [FromQuery] string repo = "scdfiles")
        {
            try
            {
                var content = await GitController.getFile(path, repo);
                var res = content.ElementAt<RepositoryContent>(0);

                return Ok(res.Content);
            }
            catch (Exception e)
            {
                return BadRequest("Error getting file.");
            }
        }

        [HttpGet("registerJson/getState")]
        public async Task<IActionResult> getState([FromQuery] string name, [FromQuery] string repo = "scdfiles")
        {
            string path = "installations/" + name + "/" + name + ".json";

            try
            {
                var content = await GitController.getFile(path, repo);
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

        [HttpPost("start")]
        public async Task<IActionResult> startInstallation()
        {
            var success = await sim.StartInstallation();
            if(success)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("stop")]
        public async Task<IActionResult> stopInstallation()
        {
            var success = await sim.StopInstallation();
            if(success)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
