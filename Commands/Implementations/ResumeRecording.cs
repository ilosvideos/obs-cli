using obs_cli.Data;
using obs_cli.Helpers;
using obs_cli.Objects;
using obs_cli.Objects.Obs;
using obs_cli.Services;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class ResumeRecording : BaseStartRecording
    {
        public override string Name => AvailableCommand.ResumeRecording.GetDescription();

        public ResumeRecording(IDictionary<string, string> arguments)
            : base(arguments) { }

        public override void Execute()
        {
            VideoService.ResetVideoInfo(new ResetVideoInfoParameters
            {
                CropTop = CropTop,
                CropRight = CropRight,
                CropLeft = CropLeft,
                CropBottom = CropBottom,
                FrameRate = FrameRate,
                OutputWidth = OutputWidth,
                OutputHeight = OutputHeight,
                CanvasWidth = CanvasWidth,
                CanvasHeight = CanvasHeight,
                ScreenToRecordHandle = ScreenToRecordHandle
            });

            ObsOutputAndEncoders outputAndEncoders = ObsService.CreateNewObsOutput();
            Store.Data.Record.OutputAndEncoders = outputAndEncoders;
            Store.Data.Record.OutputAndEncoders.obsOutput.Start();

            FileWriteService.WriteLineToFile("recording resumed");
        }
    }
}
