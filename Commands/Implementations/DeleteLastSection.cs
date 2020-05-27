using obs_cli.Data;
using obs_cli.Enums;
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
            while (Store.Data.Record.OutputAndEncoders.obsOutput != null && Store.Data.Record.OutputAndEncoders.obsOutput.Active)
            {
                Thread.Sleep(100);
            }

            FileInfo lastFile = Store.Data.Record.RecordedFiles.Last();
            lastFile.Delete();
            Store.Data.Record.RecordedFiles.Remove(lastFile);

            EmitService.EmitDeleteLastSectionResponse(Store.Data.Record.RecordedFiles.Count, true);
        }
    }
}
