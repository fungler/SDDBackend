﻿using System;
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

        [HttpPost("registerJson")]
        public async Task<IActionResult> postJson([FromBody] InstallationRoot payload, [FromQuery] string repo = "scdfiles")
        {  
            try
            {
                // create installation with a 10% chance of failing
                InstallationSim instSim = simHandler.createFailedInstallationByChance(payload, 10);
                StatusType status = await instSim.runSetup();

                if (status == StatusType.STATUS_FINISHED_SUCCESS)
                {
                    var jsonString = JsonConvert.SerializeObject(payload, Formatting.Indented);
                    await GitController.createFile("Create: " + payload.installation.name, jsonString, "./installations/" + payload.installation.name + "/" + payload.installation.name + ".json", repo);
                    return Ok("{\"status\": 200, \"message\": \"Success.\", \"installation_status\": \"" + status +"\"}");
                }
                else
                {
                    return BadRequest("{\"status\": 400, \"message\": \"Creation of installation failed.\", \"installation_status\": \"" + StatusType.STATUS_FINISHED_FAILED + "\"}");
                }
            }
            catch (ApiValidationException)
            {
                return BadRequest("{\"status\": 400, \"message\": \"File already exists in github repo.\", \"installation_status\": \"" + StatusType.STATUS_FINISHED_FAILED + "\"}");
            }
            catch (Exception)
            {
                return BadRequest("{\"status\": 400, \"message\": \"Unknown error.\", \"installation_status\": \"" + StatusType.STATUS_FINISHED_FAILED + "\"}");
            }
        }

        [HttpPost("registerJson/copy")]
        public async Task<IActionResult> copyJson([FromBody] CopyData data, [FromQuery] string repo = "scdfiles")
        {
            try
            {
                IActionResult actionResult = await getJson(data.oldName, repo);
                var content = actionResult as OkObjectResult;

                string jsonString = content.Value.ToString();
                jsonString = jsonString.Replace(data.oldName, data.newName);

                InstallationRoot newInstallation = JsonConvert.DeserializeObject<InstallationRoot>(jsonString);
                // create installation with a 10% chance of failing
                InstallationSim sim = simHandler.createFailedInstallationByChance(newInstallation, 10);
                await sim.runSetup();

                if (sim.status == StatusType.STATUS_FINISHED_SUCCESS)
                    await GitController.CopyFile(data.newName, data.oldName, repo, jsonString);
                else
                    return BadRequest("{\"status\": 400, \"message\": \"Failed to create file.\", \"installation_status\": \"" + sim.status + "\"}");
            }
            catch (NullReferenceException)
            {
                return BadRequest("{\"status\": 400, \"message\": \"Could not find file with the given filename.\"}");
            }
            catch (ApiValidationException)
            {
                return BadRequest("{\"status\": 400, \"message\": \"File already exists in github repo.\"}");
            }
            catch (Exception)
            {
                return BadRequest("{\"status\": 400, \"message\": \"Unknown error.\"}");
            }

            return Ok("{\"status\": 200, \"message\": \"Success.\", \"installation_status\": \"" + StatusType.STATUS_FINISHED_SUCCESS + "\"}");
        }

        [HttpGet("registerJson/getFile")]
        public async Task<IActionResult> getJson([FromQuery] string instName, [FromQuery] string repo = "scdfiles")
        {
            string path = "./installations/" + instName + "/" + instName + ".json";
            try
            {
                var content = await GitController.getFile(path, repo);
                var res = content.ElementAt<RepositoryContent>(0);

                return Ok(res.Content);
            }
            catch (Exception)
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
            catch (Exception)
            {
                return BadRequest("Error getting state.");
            }
        }

        [HttpPost("start")]
        public async Task<IActionResult> startInstallation([FromBody] StartStopData data)
        {
            StatusType status =  await InstallationSim.StartInstallation();

            if(status == StatusType.STATUS_RUNNING)
                return Ok("{\"status\": 200, \"message\": \"Success.\", \"installation_status\": \"" + status + "\"}");
            else
                return BadRequest("{\"status\": 400, \"message\": \"Failed to start installation.\", \"installation_status\": \"" + status + "\"}");
        }

        [HttpPost("stop")]
        public async Task<IActionResult> stopInstallation([FromBody] StartStopData data)
        {
            StatusType status = await InstallationSim.StopInstallation();

            if (status == StatusType.STATUS_STOPPED)
                return Ok("{\"status\": 200, \"message\": \"Success.\", \"installation_status\": \"" + status + "\"}");
            else
                return BadRequest("{\"status\": 400, \"message\": \"Failed to start installation.\", \"installation_status\": \"" + status + "\"}");

        }
    }
}
