using obs_cli.Data;
using obs_cli.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace obs_cli.Commands.Implementations
{
    public class PauseRecording : ICommand
    {
        public static string Name
        {
            get
            {
                return "pause-recording";
            }
        }

        public PauseRecording(IDictionary<string, string> arguments)
        {
            
        }

        public void Execute()
        {
            FileWriteService.WriteLineToFile("start pause recording");
            Store.Data.Record.OutputAndEncoders.obsOutput.Stop();
        }
    }
}
