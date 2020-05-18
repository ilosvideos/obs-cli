using obs_cli.Enums;
using obs_cli.Helpers;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class CaptureMouseClick : BaseCommand
    {
        public override string Name => AvailableCommand.CaptureMouseClick.GetDescription();

        public CaptureMouseClick(IDictionary<string, string> arguments)
        {

        }

        public override void Execute()
        {
            FileWriteService.WriteLineToFile("in CaptureMouseClick");
        }
    }
}
