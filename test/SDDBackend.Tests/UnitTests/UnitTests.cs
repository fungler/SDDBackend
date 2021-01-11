using SDDBackend.Handlers;
using SDDBackend.Models;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests
{
    public class UnitTests
    {
        private readonly InstallationSimHandler simHandler = InstallationSimHandler.GetInstance();
        private readonly ITestOutputHelper output;

        public UnitTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("success installation 1", 1000, 1000)]
        [InlineData("success installation 2", 2000, 2000)]
        public async Task createInstallation_installations_success(string inst, int startTime, int runTime)
        {
            // sending output as func param to avoid test output bugs compared to making a class field in simHandler
            InstallationSim i1 = simHandler.createSuccessfulInstallation(inst, startTime, runTime, output);

            // use whenall to make it run parallel and speed up mulitple setups
            await Task.WhenAll(
                Task.Run(() => i1.runSetup())
            );

            Assert.Equal(StatusType.STATUS_FINISHED_SUCCESS, i1.status);
        }


        [Fact]
        public async Task createInstallation_installation_fail()
        {
            InstallationSim i1 = simHandler.createFailedInstallation("failed installation 1", 1000, 1000, 1000, output);

            await Task.Run(async () =>
            {
                await i1.runSetup();
            });

            Assert.Equal(StatusType.STATUS_FINISHED_FAILED, i1.status);
        }

        [Fact]
        public async Task createInstallation_installations_success_and_fail()
        {
            InstallationSim i1 = simHandler.createSuccessfulInstallation("success installation 3", 1000, 1000, output);
            InstallationSim i2 = simHandler.createFailedInstallation("failed installation 2", 1000, 1000, 1000, output);

            // use whenall to make it run parallel and speed up mulitple setups
            await Task.WhenAll(
                Task.Run(() => i1.runSetup()),
                Task.Run(() => i2.runSetup())
            );

            Assert.Equal(StatusType.STATUS_FINISHED_SUCCESS, i1.status);
            Assert.Equal(StatusType.STATUS_FINISHED_FAILED, i2.status);
        }

        [Fact]
        public void create_random_installation_success()
        {
            InstallationRoot testInst = new InstallationRoot();
            // set data of installation to avoid null reference
            testInst.installation = new Installation();
            testInst.installation.name = "TEST_INSTALLATION";

            InstallationSim instSim = simHandler.createFailedInstallationByChance(testInst, 0);
            Assert.False(instSim.shouldFail);
        }

        [Fact]
        public void create_random_installation_fail()
        {
            InstallationRoot testInst = new InstallationRoot();
            // set data of installation to avoid null reference
            testInst.installation = new Installation();
            testInst.installation.name = "TEST_INSTALLATION";

            InstallationSim instSim = simHandler.createFailedInstallationByChance(testInst, 100);
            Assert.True(instSim.shouldFail);
        }

        [Fact]
        public void create_random_installations()
        {
            int numOfFailedInstallations = 0;

            InstallationRoot testInst = new InstallationRoot();
            // set data of installation to avoid null reference
            testInst.installation = new Installation();
            testInst.installation.name = "TEST_INSTALLATION";

            for (int i = 0; i < 1000; i++)
            {
                InstallationSim instSim = simHandler.createFailedInstallationByChance(testInst, 50);

                if (instSim.shouldFail)
                    numOfFailedInstallations++;
            }

            // Assume that at least 1 installation will fail on a thousand runs with a 50% chance of failing
            Assert.True(numOfFailedInstallations > 0);
        }
    }
}