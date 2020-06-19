using obs_cli.Data;
using obs_cli.Enums;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class DisableWebcamOnly : BaseCommand
    {
        public override string Name => AvailableCommand.DisableWebcamOnly.GetDescription();

        public DisableWebcamOnly(IDictionary<string, string> arguments)
        {

        }

        public override void Execute()
        {
            Store.Data.Webcam.IsWebcamOnly = false;
        }
    }
}
