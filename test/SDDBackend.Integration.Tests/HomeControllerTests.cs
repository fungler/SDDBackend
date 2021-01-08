using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using SDDBackend.Controllers;
using SDDBackend.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace SDDBackend.Integration.Tests
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class TestControllerTest : IClassFixture<WebApplicationFactory<SDDBackend.Startup>>
    {
        private readonly WebApplicationFactory<SDDBackend.Startup> _factory;

        public TestControllerTest(WebApplicationFactory<SDDBackend.Startup> factory)
        {
            _factory = factory;
        }

        [Theory, Priority(1)]
        [InlineData("api/")]

        public async Task GetEndpoints(string url)
        {
            var client = _factory.CreateClient();

            var res = await client.GetAsync(url);

            Assert.Equal<HttpStatusCode>(HttpStatusCode.OK, res.StatusCode);
        }


        [Theory, Priority(2)]
        [InlineData("api/home/registerJson?path=installations/TESTING-INSTALLATION/TESTING-INSTALLATION.json&repo=scdfiles_test")]
        public async Task CreateJsonDocument(string url)
        {
            string testJson = "{" + Environment.NewLine +
"    \"azureTenant\": \"simcorp.onmicrosoft.com\"," + Environment.NewLine +
"    \"subscriptionId\": \"d6741d73-abee-41f5-b0f5-886bd849a2b2\"," + Environment.NewLine +
"    \"domainName\": \"sdddev.simcorpext.net\"," + Environment.NewLine +
"    \"network\": {" + Environment.NewLine +
"        \"vnetResourceGroupName\": \"RG-Network\"," + Environment.NewLine +
"        \"vnetName\": \"10.205.0.0_22\"," + Environment.NewLine +
"        \"subnetName\": \"Tenant.WestEurope-subnet\"" + Environment.NewLine +
"    }," + Environment.NewLine +
"    \"secrets\": {" + Environment.NewLine +
"        \"keyVaultName\": \"tenantwesteuropedev\"," + Environment.NewLine +
"        \"adAutomationAccountPrefix\": \"serviceAccount\"," + Environment.NewLine +
"        \"dbAutomationAccountPrefix\": \"dbSys\"" + Environment.NewLine +
"    }," + Environment.NewLine +
"    \"installation\": {" + Environment.NewLine +
"        \"name\": \"TESTING-INSTALLATION\"," + Environment.NewLine +
"        \"resourceGroupName\": \"TESTING-INSTALLATION\"," + Environment.NewLine +
"        \"location\": \"WestEurope\"," + Environment.NewLine +
"        \"storageAccountName\": \"scinst01storage\"," + Environment.NewLine +
"        \"adminsGroupName\": \"TESTING-INSTALLATION_Admins\"," + Environment.NewLine +
"        \"fullyQualifiedGmsaName\": \"sdddev.simcorpext.net\\\\TESTING-INSTALLATION$\"," + Environment.NewLine +
"        \"gmsaHostsGroupDistinguishedName\": \"CN=TESTING-INSTALLATION_GMSAHosts,OU=TESTING-INSTALLATION,OU=Installations,OU=Tenant,DC=sdddev,DC=simcorpext,DC=net\"," + Environment.NewLine +
"        \"gmsaHostsGroupName\": \"TESTING-INSTALLATION_GMSAHosts\"," + Environment.NewLine +
"        \"gmsaName\": \"TESTING-INSTALLATION\"," + Environment.NewLine +
"        \"organizationalUnitDistinguishedName\": \"OU=TESTING-INSTALLATION,OU=Installations,OU=Tenant,DC=sdddev,DC=simcorpext,DC=net\"," + Environment.NewLine +
"        \"serverLocalAdminGroup\": \"sdddev.simcorpext.net\\\\Oper_ServerLocal_Admin\"," + Environment.NewLine +
"        \"usersGroupName\": \"TESTING-INSTALLATION_Users\"," + Environment.NewLine +
"        \"keyVaultName\": \"sc-TESTING-INSTALLATION-keyvault\"," + Environment.NewLine +
"        \"databaseServer\": {" + Environment.NewLine +
"            \"hostFqdn\": \"db01.sdddev.simcorpext.net\"," + Environment.NewLine +
"            \"ipAddress\": \"10.205.0.7\"," + Environment.NewLine +
"            \"protocol\": \"TCP\"," + Environment.NewLine +
"            \"port\": 1521," + Environment.NewLine +
"            \"serviceName\": \"DB01A\"," + Environment.NewLine +
"            \"ordsHttpsPort\": 8443" + Environment.NewLine +
"        }," + Environment.NewLine +
"        \"database\": {" + Environment.NewLine +
"            \"pdbName\": \"inst01\"," + Environment.NewLine +
"            \"creationMethod\": \"SNAPSHOTCOPY\"" + Environment.NewLine +
"        }," + Environment.NewLine +
"        \"localFileSharePath\": \"C:\\\\Shares\\\\TESTING-INSTALLATION\"," + Environment.NewLine +
"        \"netRootUncPath\": \"\\\\\\\\sc-TESTING-INSTALLATION-01.sdddev.simcorpext.net\\\\TESTING-INSTALLATION\\\\NetRoot\"," + Environment.NewLine +
"        \"orderManager\": {" + Environment.NewLine +
"            \"sqlServerFqdn\": \"sc-TESTING-INSTALLATION-sql-server.database.windows.net\"," + Environment.NewLine +
"            \"sqlDatabaseName\": \"FixNET\"" + Environment.NewLine +
"        }," + Environment.NewLine +
"        \"mucsPort\": \"59152\"," + Environment.NewLine +
"        \"svcDirectoryServicePort\": \"59153\"," + Environment.NewLine +
"        \"source\": {" + Environment.NewLine +
"            \"fileSystem\": {" + Environment.NewLine +
"                \"storageAccount\": {" + Environment.NewLine +
"                    \"subscriptionId\": \"15b43e35-0aa9-4c01-85e6-6b99606ea650\"," + Environment.NewLine +
"                    \"resourceGroupName\": \"SDD.Shared\"," + Environment.NewLine +
"                    \"name\": \"sddshareddev\"," + Environment.NewLine +
"                    \"containerName\": \"sourcesetfiles\"," + Environment.NewLine +
"                    \"path\": \"f8978fca-2a82-44e8-aef3-de01941a8af4/ScdFileSystem\"" + Environment.NewLine +
"                }" + Environment.NewLine +
"            }," + Environment.NewLine +
"            \"database\": {" + Environment.NewLine +
"                \"type\": \"ReadOnlyPDB\"," + Environment.NewLine +
"                \"dbServerName\": \"db01\"," + Environment.NewLine +
"                \"dbInstanceName\": \"DB01A\"," + Environment.NewLine +
"                \"pdbName\": \"GSSTARTER2001\"" + Environment.NewLine +
"            }" + Environment.NewLine +
"        }," + Environment.NewLine +
"        \"state\": \"GLOBAL_STD_TEST\"," + Environment.NewLine +
"        \"iconColor\": \"red\"," + Environment.NewLine +
"        \"tags\": {" + Environment.NewLine +
"            \"creator\": \"CCOE\"," + Environment.NewLine +
"            \"cost-center\": \"Cloud Center of Excellence\"," + Environment.NewLine +
"            \"legal-unit\": \"1000 SimCorp A/S\"," + Environment.NewLine +
"            \"environment\": \"dev\"," + Environment.NewLine +
"            \"vm-backup\": \"none\"," + Environment.NewLine +
"            \"sql-backup\": \"none\"," + Environment.NewLine +
"            \"customer-data\": \"no\"," + Environment.NewLine +
"            \"personal-data\": \"no\"," + Environment.NewLine +
"            \"expiration-date\": \"none\"," + Environment.NewLine +
"            \"service-window\": \"none\"," + Environment.NewLine +
"            \"application\": \"none\"," + Environment.NewLine +
"            \"client-shortname\": \"none\"" + Environment.NewLine +
"        }," + Environment.NewLine +
"        \"internationalSettings\": {" + Environment.NewLine +
"            \"inputLanguageId\": \"0409:00000409\"," + Environment.NewLine +
"            \"format\": \"en-US\"," + Environment.NewLine +
"            \"systemLocale\": \"en-US\"," + Environment.NewLine +
"            \"geoId\": 61" + Environment.NewLine +
"        }," + Environment.NewLine +
"        \"vmScaleSets\": [" + Environment.NewLine +
"            {" + Environment.NewLine +
"                \"name\": \"sc-TESTING-INSTALLATION-vm-scale-set\"," + Environment.NewLine +
"                \"computerNamePrefix\": \"sc-vss-\"," + Environment.NewLine +
"                \"instanceCount\": 2," + Environment.NewLine +
"                \"instance\": {" + Environment.NewLine +
"                    \"vmSize\": \"Standard_D8_v3\"," + Environment.NewLine +
"                    \"vmImageSku\": \"2019-Datacenter\"," + Environment.NewLine +
"                    \"licenseType\": \"Windows_Server\"," + Environment.NewLine +
"                    \"configurationMode\": \"ApplyAndAutoCorrect\"," + Environment.NewLine +
"                    \"roles\": [" + Environment.NewLine +
"                        {" + Environment.NewLine +
"                            \"name\": \"Agent\"," + Environment.NewLine +
"                            \"additional\": \"maxnumber=20\"" + Environment.NewLine +
"                        }" + Environment.NewLine +
"                    ]" + Environment.NewLine +
"                }" + Environment.NewLine +
"            }" + Environment.NewLine +
"        ]," + Environment.NewLine +
"        \"vms\": [" + Environment.NewLine +
"            {" + Environment.NewLine +
"                \"name\": \"sc-TESTING-INSTALLATION-01\"," + Environment.NewLine +
"                \"vmSize\": \"Standard_D8_v3\"," + Environment.NewLine +
"                \"vmImageSku\": \"2019-Datacenter\"," + Environment.NewLine +
"                \"licenseType\": \"Windows_Server\"," + Environment.NewLine +
"                \"configurationMode\": \"ApplyAndAutoCorrect\"," + Environment.NewLine +
"                \"roles\": [" + Environment.NewLine +
"                    {" + Environment.NewLine +
"                        \"name\": \"AgentMain\"," + Environment.NewLine +
"                        \"satag\": \"AgentMain\"," + Environment.NewLine +
"                        \"additional\": \"maxnumber=20\"" + Environment.NewLine +
"                    }," + Environment.NewLine +
"                    {" + Environment.NewLine +
"                        \"name\": \"FileServer\"" + Environment.NewLine +
"                    }," + Environment.NewLine +
"                    {" + Environment.NewLine +
"                        \"name\": \"MUCS\"" + Environment.NewLine +
"                    }" + Environment.NewLine +
"                ]" + Environment.NewLine +
"            }," + Environment.NewLine +
"            {" + Environment.NewLine +
"                \"name\": \"sc-TESTING-INSTALLATION-02\"," + Environment.NewLine +
"                \"vmSize\": \"Standard_D8_v3\"," + Environment.NewLine +
"                \"vmImageSku\": \"2019-Datacenter\"," + Environment.NewLine +
"                \"licenseType\": \"Windows_Server\"," + Environment.NewLine +
"                \"configurationMode\": \"ApplyAndAutoCorrect\"," + Environment.NewLine +
"                \"roles\": [" + Environment.NewLine +
"                    {" + Environment.NewLine +
"                        \"name\": \"AgentMain\"," + Environment.NewLine +
"                        \"satag\": \"omserver\"," + Environment.NewLine +
"                        \"additional\": \"maxnumber=20\"" + Environment.NewLine +
"                    }," + Environment.NewLine +
"                    {" + Environment.NewLine +
"                        \"name\": \"MUCSFailover\"" + Environment.NewLine +
"                    }," + Environment.NewLine +
"                    {" + Environment.NewLine +
"                        \"name\": \"FixNET\"" + Environment.NewLine +
"                    }" + Environment.NewLine +
"                ]" + Environment.NewLine +
"            }" + Environment.NewLine +
"        ]" + Environment.NewLine +
"    }" + Environment.NewLine +
"}";


            var client = _factory.CreateClient();

            var res = await client.PostAsync(url, new StringContent(testJson, Encoding.UTF8, "application/json"));

            Assert.Equal<HttpStatusCode>(HttpStatusCode.OK, res.StatusCode);
        }

        [Theory, Priority(3)]
        [InlineData("api/home/registerJson/getFile?instName=TESTING-INSTALLATION&repo=scdfiles_test")]
        public async Task GetInstallation(string url)
        {
            var client = _factory.CreateClient();

            var res = await client.GetAsync(url);

            Assert.Equal<HttpStatusCode>(HttpStatusCode.OK, res.StatusCode);
        }

        [Theory, Priority(4)]
        [InlineData("api/home/registerJson/copy?repo=scdfiles_test")]
        public async Task CreateJsonCopyDocument(string url)
        {
            string testJson = "{" + Environment.NewLine +
            "    \"oldName\": \"TESTING-INSTALLATION\"," + Environment.NewLine +
            "    \"newName\": \"TESTING-INSTALLATION-COPY\"," + Environment.NewLine +
            "    \"copyMethod\": \"Full\"," + Environment.NewLine +
            "    \"client\": {" + Environment.NewLine +
            "        \"name\": \"Danske Bank\"," + Environment.NewLine +
            "        \"id\": \"1\"" + Environment.NewLine +
            "    }" + Environment.NewLine +
            "}";


            var client = _factory.CreateClient();

            var res = await client.PostAsync(url, new StringContent(testJson, Encoding.UTF8, "application/json"));

            Assert.Equal<HttpStatusCode>(HttpStatusCode.OK, res.StatusCode);
        }

        [Theory, Priority(5)]
        [InlineData("api/home/registerJson/getState?name=TESTING-INSTALLATION&repo=scdfiles_test")]

        public async Task GetInstallationStatus(string url)
        {
            var client = _factory.CreateClient();

            var res = await client.GetAsync(url);

            Assert.Equal<HttpStatusCode>(HttpStatusCode.OK, res.StatusCode);
        }
        
        [Fact, Priority(6)]
        public async Task RemoveTestJsonDocuments()
        {
            bool bSuccess = await GitController.removeFile("installations/TESTING-INSTALLATION/TESTING-INSTALLATION.json", "scdfiles_test");
            bool bSuccessCopy = await GitController.removeFile("installations/TESTING-INSTALLATION-COPY/TESTING-INSTALLATION-COPY.json", "scdfiles_test");
            Assert.True(bSuccess);
            Assert.True(bSuccessCopy);
        }
    }

}
