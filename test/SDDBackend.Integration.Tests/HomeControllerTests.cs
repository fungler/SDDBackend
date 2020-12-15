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

    public class TestControllerTest : IClassFixture<WebApplicationFactory<SDDBackend.Startup>>
    {
        private readonly WebApplicationFactory<SDDBackend.Startup> _factory;

        public TestControllerTest(WebApplicationFactory<SDDBackend.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("api/")]

        public async Task GetEndpoints(string url)
        {
            var client = _factory.CreateClient();

            var res = await client.GetAsync(url);

            Assert.Equal<HttpStatusCode>(HttpStatusCode.OK, res.StatusCode);
        }
    }
}
