using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Helpers;
using obs_cli.Services;
using obs_cli.Services.Recording;
using obs_cli.Services.Recording.Abstract;
using obs_cli.Services.Recording.Objects;
using System;
using System.Collections.Generic;
using System.Threading;

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

            // sleep to make sure the pausing is complete
            while (Store.Data.Record.OutputAndEncoders.obsOutput != null)
            {
                Thread.Sleep(100);
            }
            
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

            EmitService.EmitStatusResponse(AvailableCommand.ResumeRecording, isStarted);
        }
    }
}
