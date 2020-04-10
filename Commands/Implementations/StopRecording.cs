using obs_cli.Data;
using obs_cli.Objects;
using System.Collections.Generic;
using System.Timers;

namespace obs_cli.Commands.Implementations
{
    public class StopRecording : ICommand
    {
        public static string Name
        {
            get
            {
                return "stop-recording";
            }
        }

        private Timer OutputStopTimer { get; set; }

        public StopRecording(IDictionary<string, string> arguments)
        {
            OutputStopTimer = new Timer();
        }

        public void Execute()
        {
            Store.Data.Obs.OutputAndEncoders.obsOutput.Stop();

            OutputStopTimer.Interval = 50;
            OutputStopTimer.Elapsed += new ElapsedEventHandler(StopRecordingWhenOutputInactive);
            OutputStopTimer.Enabled = true;
            OutputStopTimer.Start();
        }

        private void StopRecordingWhenOutputInactive(object source, ElapsedEventArgs e)
        {
            if (Store.Data.Obs.OutputAndEncoders.obsOutput != null && Store.Data.Obs.OutputAndEncoders.obsOutput.Active)
            {
                return;
            }

            OutputStopTimer.Stop();
            OutputStopTimer.Dispose();
            OutputStopTimer = null;

            Store.Data.Obs.OutputAndEncoders.Dispose();

            new VideoMerge(Store.Data.Record.RecordedFiles).CombineAndWrite();
            Store.Data.ResetRecordModule();
        }
    }
}
