using obs_cli.Data;
using obs_cli.Helpers;
using obs_cli.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

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

            while (Store.Data.Record.OutputAndEncoders.obsOutput != null && Store.Data.Record.OutputAndEncoders.obsOutput.Active)
            {
                Thread.Sleep(100);
            }

            FileInfo lastFile = Store.Data.Record.RecordedFiles.Last();
            lastFile.Delete();
            Store.Data.Record.RecordedFiles.Remove(lastFile);

            if (Store.Data.Record.RecordedFiles.Count == 0)
            {
                VideoService.CancelRecording();
            }
        }
    }
}
