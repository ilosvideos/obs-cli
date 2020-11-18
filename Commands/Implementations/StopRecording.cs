using obs_cli.Commands.Abstract;
using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Objects;
using obs_cli.Services;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class StopRecording : BaseOutputStop
    {
        public override string Name => AvailableCommand.StopRecording.GetDescription();

        public StopRecording(IDictionary<string, string> arguments) { }

        public override void Execute()
        {
            // if the recorder is paused, we don't need to dispose the encoders since the pause disposed them.
            if (!Store.Data.Record.IsPaused)
            {
                this.StopOutput();
            }
        }

        protected override void OutputStopped()
        {
            VideoMergeOutput output = new VideoMerge(Store.Data.Record.RecordedFiles).CombineAndWrite();

            Store.Data.ResetRecordModule();

            EmitService.EmitStopRecordingStatusResponse(output.FileOutputPath, Store.Data.Record.LastVideoName, output.IsSuccessful, output.MergeFailureReason);
        }
    }
}
