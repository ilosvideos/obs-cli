using obs_cli.Helpers;
using System;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class Terminate : ICommand
    {
        public static string Name
        {
            get
            {
                return "terminate";
            }
        }

        public Terminate(IDictionary<string, string> arguments)
        {

        }

        public void Execute()
        {
            FileWriteService.WriteLineToFile("terminating");
            Environment.Exit(0);
        }
    }
}
