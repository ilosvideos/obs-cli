using obs_cli.Commands.Abstract;
using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Services;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class PauseRecording : BaseOutputStop
    {
        public override string Name => AvailableCommand.PauseRecording.GetDescription();

        public PauseRecording(IDictionary<string, string> arguments) { }

        public override void Execute()
        {
            if (Store.Data.Record.IsPausing)
            {
                return;
            }

            Store.Data.Record.IsPausing = true;

            this.StopOutput();

            EmitService.EmitStatusResponse(AvailableCommand.PauseRecording, true, "Paused");
        }

        protected override void OutputStopped()
        {
            Store.Data.Record.IsPausing = false;
            Store.Data.Record.IsPaused = true;
        }
    }
}
