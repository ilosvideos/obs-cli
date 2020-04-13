using obs_cli.Data;
using obs_cli.Objects;
using System.Collections.Generic;
using System.Timers;

namespace obs_cli.Commands.Implementations
{
    public class StopRecording : BaseCommand
    {
        public override string Name => AvailableCommand.StopRecording.GetDescription();

        private Timer OutputStopTimer { get; set; }

        public StopRecording(IDictionary<string, string> arguments)
        {
            OutputStopTimer = new Timer();
        }

        public override void Execute()
        {
            Store.Data.Record.OutputAndEncoders.obsOutput.Stop();

            OutputStopTimer.Interval = 50;
            OutputStopTimer.Elapsed += new ElapsedEventHandler(DisposeEncodersAndFinalizeVideo);
            OutputStopTimer.Enabled = true;
            OutputStopTimer.Start();
        }

        private void DisposeEncodersAndFinalizeVideo(object source, ElapsedEventArgs e)
        {
            if (Store.Data.Record.OutputAndEncoders.obsOutput != null && Store.Data.Record.OutputAndEncoders.obsOutput.Active)
            {
                return;
            }

            OutputStopTimer.Stop();
            OutputStopTimer.Dispose();
            OutputStopTimer = null;

            Store.Data.Record.OutputAndEncoders.Dispose();

            new VideoMerge(Store.Data.Record.RecordedFiles).CombineAndWrite();
            Store.Data.ResetRecordModule();
        }
    }
}
