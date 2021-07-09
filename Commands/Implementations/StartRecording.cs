using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Objects;
using obs_cli.Services;
using obs_cli.Services.Recording;
using obs_cli.Services.Recording.Abstract;
using obs_cli.Services.Recording.Objects;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class StartRecording : BaseStartRecording
    {
        public string VideoOutputFolder { get; set; }

        public override string Name => AvailableCommand.StartRecording.GetDescription();

        public StartRecording(IDictionary<string, string> arguments)
            : base(arguments)
        {
            this.VideoOutputFolder = arguments["videoOutputFolder"];
        }

        public override void Execute()
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
                VideoOutputFolder = VideoOutputFolder,
                ScreenX = ScreenX,
                ScreenY = ScreenY
            };

            Loggers.CliLogger.Info($"StartRecording IsWebcamOnly: {Store.Data.Webcam.IsWebcamOnly}");

            IBaseRecordingService service = RecordingFactory.Make(Store.Data.Webcam.IsWebcamOnly, baseRecordingParameters);
            var isStarted = service.StartRecording();

            EmitService.EmitStatusResponse(AvailableCommand.StartRecording, isStarted);
        }
    }
}
