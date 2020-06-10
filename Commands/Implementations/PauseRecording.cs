using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Services;
using System.Collections.Generic;
using System.Timers;

namespace obs_cli.Commands.Implementations
{
    public class PauseRecording : BaseCommand
    {
        public Timer OutputPauseTimer { get; set; }

        public override string Name => AvailableCommand.PauseRecording.GetDescription();

        public PauseRecording(IDictionary<string, string> arguments)
        {
            OutputPauseTimer = new Timer();
        }

        public override void Execute()
        {
            if (Store.Data.Record.IsPausing)
            {
                return;
            }

            Store.Data.Record.OutputAndEncoders.obsOutput.Stop();

            OutputPauseTimer = new Timer();
            OutputPauseTimer.Interval = 50;
            OutputPauseTimer.Elapsed += new ElapsedEventHandler(PauseRecordingWhenOutputInactive);
            OutputPauseTimer.Enabled = true;
            OutputPauseTimer.Start();

            Store.Data.Record.IsPausing = true;
            EmitService.EmitStatusResponse(AvailableCommand.PauseRecording, true, "Paused");
        }

        private void PauseRecordingWhenOutputInactive(object source, ElapsedEventArgs e)
        {
            if (Store.Data.Record.OutputAndEncoders.obsOutput != null && Store.Data.Record.OutputAndEncoders.obsOutput.Active)
            {
                return;
            }

            OutputPauseTimer.Stop();
            OutputPauseTimer.Dispose();
            OutputPauseTimer = null;

            Store.Data.Record.OutputAndEncoders.Dispose();
            Store.Data.Record.IsPausing = false;
            Store.Data.Record.IsPaused = true;
        }
    }
}
