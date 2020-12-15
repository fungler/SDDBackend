using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SDDBackend.Integration.Tests
{

    public class HomeControllerTests : WebApplicationFactory<Startup>
    {
        private readonly HttpClient client;

        public HomeControllerTests()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            client = appFactory.WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot("./src/SDDBackend")).CreateClient();
        }

        [Fact]
        public async Task GetHttpRequest()
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:7001/api/home/registerJson/getState?name=inst-sim-01");
            HttpStatusCode status = response.StatusCode;

            // Assert 
            Assert.Equal<HttpStatusCode>(HttpStatusCode.OK, status);
        }

    }
}
