using obs_cli.Data;
using obs_cli.Helpers;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class PauseRecording : BaseCommand
    {
        public override string Name
        {
            get
            {
                return AvailableCommand.PauseRecording.GetDescription();
            }
        }

        public PauseRecording(IDictionary<string, string> arguments)
        {
            
        }

        public override void Execute()
        {
            FileWriteService.WriteLineToFile("start pause recording");
            Store.Data.Record.OutputAndEncoders.obsOutput.Stop();
        }
    }
}
