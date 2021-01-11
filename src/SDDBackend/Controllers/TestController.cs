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
    [Route("api")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Running";
        }
    }
}