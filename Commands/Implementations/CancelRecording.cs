using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Services;
using System.Collections.Generic;
using System.Timers;

namespace obs_cli.Commands.Implementations
{
    public class CancelRecording : BaseCommand
    {
        public override string Name => AvailableCommand.CancelRecording.GetDescription();

        public Timer OutputStopTimer { get; set; }

        public CancelRecording(IDictionary<string, string> arguments)
        {
            OutputStopTimer = new Timer();
        }

        public override void Execute()
        {
            VideoService.CancelRecording();

            OutputStopTimer = new Timer();
            OutputStopTimer.Interval = 50;
            OutputStopTimer.Elapsed += new ElapsedEventHandler(EmitStatusResponseWhenOutputInactive);
            OutputStopTimer.Enabled = true;
            OutputStopTimer.Start();
        }

        private void EmitStatusResponseWhenOutputInactive(object source, ElapsedEventArgs e)
        {
            if (Store.Data.Record.OutputAndEncoders.obsOutput != null && Store.Data.Record.OutputAndEncoders.obsOutput.Active)
            {
                return;
            }

            OutputStopTimer.Stop();
            OutputStopTimer.Dispose();
            OutputStopTimer = null;

            EmitService.EmitStatusResponse(AvailableCommand.CancelRecording, true);
        }
    }
}
