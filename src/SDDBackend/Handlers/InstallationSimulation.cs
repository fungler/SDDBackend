using System.Threading.Tasks;
using System;

namespace SDDBackend.Handlers
{
    public class InstallationSimulation
    {

        private Random rnd;

        public InstallationSimulation()
        {
            rnd = new Random();
        }

        public async Task<bool> StartInstallation()
        {
            var minDelay = 1000;
            var maxDelay = 10000;

            var successPercent = 90;

            var delay = rnd.Next(minDelay, maxDelay+1);
            var success = rnd.Next(1, 101);

            await Task.Delay(delay);
            return success <= successPercent; // 1-90 = success
        }

        public async Task<bool> StopInstallation()
        {
            var minDelay = 1000;
            var maxDelay = 10000;
            var successPercent = 80;

            var delay = rnd.Next(minDelay, maxDelay+1);
            var success = rnd.Next(1, 101);

            await Task.Delay(delay);
            return success <= successPercent; // 1-90 = success
        }
    }
}