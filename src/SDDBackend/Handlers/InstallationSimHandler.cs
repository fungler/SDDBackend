using SDDBackend.Models;
using System;
using Xunit.Abstractions;

namespace SDDBackend.Handlers
{
    public class InstallationSimHandler
    {
        private static InstallationSimHandler instance = new InstallationSimHandler();
        static InstallationSimHandler() { }
        private InstallationSimHandler() { }

        Random rand = new Random();

        public static InstallationSimHandler GetInstance()
        {
            return instance;
        }




        public InstallationSim createSuccessfulInstallation(InstallationRoot installation, int startTime, int runTime, ITestOutputHelper output)
        {
            InstallationSim installationSuccess = new InstallationSim(installation, startTime, runTime, false, 0, output);
            return installationSuccess;
        }
        public InstallationSim createSuccessfulInstallation(InstallationRoot installation, int startTime, int runTime)
        {
            InstallationSim installationSuccess = new InstallationSim(installation, startTime, runTime, false, 0);
            return installationSuccess;
        }

        public InstallationSim createSuccessfulInstallation(string name, int startTime, int runTime, ITestOutputHelper output)
        {
            InstallationSim installationSuccess = new InstallationSim(name, startTime, runTime, false, 0, output);
            return installationSuccess;
        }
        public InstallationSim createSuccessfulInstallation(string name, int startTime, int runTime)
        {
            InstallationSim installationSuccess = new InstallationSim(name, startTime, runTime, false, 0);
            return installationSuccess;
        }




        public InstallationSim createFailedInstallation(InstallationRoot installation, int startTime, int runTime, int failTime, ITestOutputHelper output)
        {
            InstallationSim installationFail = new InstallationSim(installation, startTime, runTime, true, failTime, output);
            return installationFail;
        }
        public InstallationSim createFailedInstallation(InstallationRoot installation, int startTime, int runTime, int failTime)
        {
            InstallationSim installationFail = new InstallationSim(installation, startTime, runTime, true, failTime);
            return installationFail;
        }

        public InstallationSim createFailedInstallation(string name, int startTime, int runTime, int failTime, ITestOutputHelper output)
        {
            InstallationSim installationFail = new InstallationSim(name, startTime, runTime, true, failTime, output);
            return installationFail;
        }
        public InstallationSim createFailedInstallation(string name, int startTime, int runTime, int failTime)
        {
            InstallationSim installationFail = new InstallationSim(name, startTime, runTime, true, failTime);
            return installationFail;
        }


        public InstallationSim createFailedInstallationByChance(InstallationRoot installation, int chance)
        {
            InstallationSim randomInstallation;

            int installationChance = rand.Next(1, 101);
            int rndMin = rand.Next(3000, 6001);
            int rndMax = rand.Next(4000, 10001);

            if (installationChance <= chance)
                randomInstallation = new InstallationSim(installation, rndMin, rndMax, true, 2000);
            else
                randomInstallation = new InstallationSim(installation, rndMin, rndMax, false, 0);

            return randomInstallation;
        }
    }
}
