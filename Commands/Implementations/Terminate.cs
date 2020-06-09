using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Helpers;
using System;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class Terminate : BaseCommand
    {
        public override string Name => AvailableCommand.Terminate.GetDescription();

        public Terminate(IDictionary<string, string> arguments)
        {
            
        }

        public override void Execute()
        {
            Store.Data.Audio.InputMeter.RemoveCallback();
            Store.Data.Audio.OutputMeter.RemoveCallback();

            FileWriteService.WriteLineToFile(Environment.NewLine);

            Environment.Exit(0);
        }
    }
}