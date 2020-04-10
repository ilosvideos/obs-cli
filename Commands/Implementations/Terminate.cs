using obs_cli.Helpers;
using System;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class Terminate : BaseCommand
    {
        public override string Name
        {
            get
            {
                return AvailableCommand.Terminate.GetDescription();
            }
        }

        public Terminate(IDictionary<string, string> arguments)
        {

        }

        public override void Execute()
        {
            FileWriteService.WriteLineToFile("terminating");
            Environment.Exit(0);
        }
    }
}
