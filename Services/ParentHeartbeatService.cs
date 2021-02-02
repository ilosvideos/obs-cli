using obs_cli.Data;
using obs_cli.Objects;
using System;
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

            var terminateAction = new Action(() =>
            {
                Loggers.CliLogger.Warn($"Parent process {Store.Data.App.ParentProcessId} is gone. Shutting CLI down.");
                _heartbeatTimer.Stop();
                Program.Terminate();
            });

            try
            {
                var parentProcess = Process.GetProcessById(Store.Data.App.ParentProcessId.Value);
                if (parentProcess == null || parentProcess.HasExited)
                {
                    Debug.WriteLine($"Parent process {Store.Data.App.ParentProcessId} is gone. Shutting CLI down.");
                    terminateAction();
                    return;
                }
            }
            catch(ArgumentException)
            {
                Debug.WriteLine($"Process {Store.Data.App.ParentProcessId} does not exist or is not running! Shutting CLI down.");
                terminateAction();
                return;
            }

            Debug.WriteLine($"Parent process {Store.Data.App.ParentProcessId} exists! Carrying on");
        }

        public static void Teardown()
        {
            _heartbeatTimer.Stop();
            _heartbeatTimer.Dispose();
            _heartbeatTimer = null;
        }
    }
}
