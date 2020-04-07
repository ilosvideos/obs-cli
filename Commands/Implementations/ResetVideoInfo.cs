using obs_cli.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace obs_cli.Commands.Implementations
{
    public class ResetVideoInfo : ICommand
    {
        public static string Name
        {
            get
            {
                return "reset-video-info";
            }
        }

        public ResetVideoInfo(IDictionary<string, string> arguments)
        {

        }

        public void Execute()
        {
            FileWriteService.WriteToFile("In ResetVideoInfo command");
        }
    }
}
