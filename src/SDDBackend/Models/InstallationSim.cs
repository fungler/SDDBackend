using System;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace SDDBackend.Models
{
    public class InstallationSim
    {

        private readonly ITestOutputHelper output;

        public InstallationRoot installation;
        public string name { get; set; }
        public StatusType status { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        // config
        public int startTimeMs { get; set; } // how long should the setup/init run for
        public int runTimeMs { get; set; } // how long should the setup run for
        public int stopTimeMs { get; set; } // how long should the setup stop for
        public bool shouldFail { get; set; } // should this installation fail
        public int failTimeMs { get; set; } // how long till the setup should fail


        public InstallationSim(InstallationRoot installation, int startTime, int runTime, bool shouldFail, int failTime)
        {
            this.installation = installation;
            this.name = installation.installation.name;
            this.status = StatusType.STATUS_COLD;
            this.creationDate = DateTime.Now;
            this.runTimeMs = runTime;
            this.startTimeMs = startTime;
            this.shouldFail = shouldFail;
            this.failTimeMs = failTime;
        }

        public InstallationSim(InstallationRoot installation, int startTime, int runTime, bool shouldFail, int failTime, ITestOutputHelper output)
        {
            this.installation = installation;
            this.name = installation.installation.name;
            this.status = StatusType.STATUS_COLD;
            this.creationDate = DateTime.Now;
            this.runTimeMs = runTime;
            this.startTimeMs = startTime;
            this.shouldFail = shouldFail;
            this.failTimeMs = failTime;
            this.output = output;
        }

        public InstallationSim(string name, int startTime, int runTime, bool shouldFail, int failTime, ITestOutputHelper output)
        {
            this.name = name;
            this.status = StatusType.STATUS_COLD;
            this.creationDate = DateTime.Now;
            this.runTimeMs = runTime;
            this.startTimeMs = startTime;
            this.shouldFail = shouldFail;
            this.failTimeMs = failTime;
            this.output = output;
        }

        public InstallationSim(string name, int startTime, int runTime, bool shouldFail, int failTime)
        {
            this.name = name;
            this.status = StatusType.STATUS_COLD;
            this.creationDate = DateTime.Now;
            this.runTimeMs = runTime;
            this.startTimeMs = startTime;
            this.shouldFail = shouldFail;
            this.failTimeMs = failTime;
        }

        private async Task<StatusType> Fail()
        {
            await Task.Delay(failTimeMs);
            endDate = DateTime.Now;
            return StatusType.STATUS_FINISHED_FAILED;
        }

        public async Task<StatusType> runSetup()
        {
            startDate = DateTime.Now;

            status = StatusType.STATUS_STARTING;
            await Task.Delay(startTimeMs);

            status = StatusType.STATUS_RUNNING;
            await Task.Delay(runTimeMs);

            if (shouldFail)
            {
                status = await Fail();
                return status;
            }
            else
            {
                endDate = DateTime.Now;
                status = StatusType.STATUS_FINISHED_SUCCESS;
                return status;
            }

        }

        public static async Task<StatusType> StartInstallation()
        {
            Random rnd = new Random();

            int minDelay = 1000;
            int maxDelay = 10000;

            int successPercent = 90;

            int delay = rnd.Next(minDelay, maxDelay + 1);
            int success = rnd.Next(1, 101);

            await Task.Delay(delay);
            if (success <= successPercent) // 1-90 = success
                return StatusType.STATUS_RUNNING;
            else
                return StatusType.STATUS_START_FAILED;
        }

        public static async Task<StatusType> StopInstallation()
        {
            Random rnd = new Random();

            int minDelay = 1000;
            int maxDelay = 10000;
            int successPercent = 80;

            int delay = rnd.Next(minDelay, maxDelay + 1);
            int success = rnd.Next(1, 101);

            await Task.Delay(delay);

            if (success <= successPercent)
                return StatusType.STATUS_STOPPED;
            else
                return StatusType.STATUS_STOP_FAILED;
        }

    }
}
