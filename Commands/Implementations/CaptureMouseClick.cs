using obs_cli.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            
        }
    }
}
