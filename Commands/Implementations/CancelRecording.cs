using obs_cli.Commands.Abstract;
using obs_cli.Enums;
using obs_cli.Services;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class CancelRecording : BaseOutputStop
    {
        public override string Name => AvailableCommand.CancelRecording.GetDescription();

        public CancelRecording(IDictionary<string, string> arguments) { }

        public override void Execute()
        {
            this.StopOutput();
        }

        protected override void OutputStopped()
        {
            VideoService.CancelRecording();
            EmitService.EmitStatusResponse(AvailableCommand.CancelRecording, true);
        }
    }
}
