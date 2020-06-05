using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Helpers;
using obs_cli.Services;
using obs_cli.Services.Recording;
using obs_cli.Services.Recording.Abstract;
using obs_cli.Services.Recording.Objects;
using System;
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
            var isStarted = false;

            try
            {
                var baseRecordingParameters = new BaseRecordingParameters
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
                    ScreenToRecordHandle = ScreenToRecordHandle,
                    VideoOutputFolder = Store.Data.Record.VideoOutputFolder
                };

                IBaseRecordingService service = RecordingFactory.Make(Store.Data.Webcam.IsWebcamOnly, baseRecordingParameters);
                isStarted = service.StartRecording();
            }
            catch (Exception ex)
            {
                FileWriteService.WriteLineToFile(ex.Message);
            }

            EmitService.EmitStatusResponse(AvailableCommand.ResumeRecording, isStarted);
        }
    }
}
