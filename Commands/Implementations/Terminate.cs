using obs_cli.Helpers;
using System;

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

        public void Execute()
        {
            FileWriteService.WriteToFile("terminating");
            Environment.Exit(0);
        }
    }
}
