using obs_cli.Data;
using System.Diagnostics;
using System.Timers;

namespace obs_cli.Services
{
    public class ParentHeartbeatService
    {
        private const int HEARTBEAT_INTERVAL = 5000;

        private static Timer _heartbeatTimer { get; set; }

        public static void Monitor()
        {
            _heartbeatTimer = new Timer();
            _heartbeatTimer.Interval = HEARTBEAT_INTERVAL;
            _heartbeatTimer.Elapsed += PulseCheck;
            _heartbeatTimer.Start();
        }

        private static void PulseCheck(object sender, ElapsedEventArgs e)
        {
            if (!Store.Data.App.ParentProcessId.HasValue)
            {
                return;
            }

            var parentProcess = Process.GetProcessById(Store.Data.App.ParentProcessId.Value);
            if (parentProcess == null)
            {
                // it's dead. shut CLI down
                Teardown();
                return;
            }

            if (parentProcess.HasExited)
            {
                // something else made it exit between the null check and now. shut CLI down
                Teardown();
                return;
            }
        }

        public static void Teardown()
        {
            _heartbeatTimer.Stop();
            _heartbeatTimer.Dispose();
            _heartbeatTimer = null;
        }
    }
}
