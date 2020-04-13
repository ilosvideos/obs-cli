using obs_cli.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace obs_cli.Commands.Implementations
{
    public class DeleteLastSection : BaseCommand
    {
        public override string Name => AvailableCommand.DeleteLastSection.GetDescription();

        public DeleteLastSection(IDictionary<string, string> arguments)
        {

        }

        public override void Execute()
        {
            FileWriteService.WriteLineToFile("start DeleteLastSection");
        }
    }
}
