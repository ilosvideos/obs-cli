using obs_cli.Data;
using obs_cli.Services;
using System.Collections.Generic;
using System.IO;

namespace obs_cli.Commands.Implementations
{
    public class CancelRecording : BaseCommand
    {
        public override string Name => AvailableCommand.CancelRecording.GetDescription();

        public CancelRecording(IDictionary<string, string> arguments)
        {

        }

        public override void Execute()
        {
            VideoService.CancelRecording();
        }
    }
}
