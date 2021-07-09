using obs_cli.Objects;
using obs_cli.Services.Recording.Abstract;
using obs_cli.Services.Recording.Objects;

namespace obs_cli.Services.Recording
{
    public class WebcamOnlyRecordingService : BaseRecordingService
    {
        public WebcamOnlyRecordingService(BaseRecordingParameters parameters)
            :base(parameters) { }

        public override void Setup()
        {
            Loggers.CliLogger.Trace("In WebcamOnlyRecordingService Setup");
            VideoService.ConfigureWebcamOnly(FrameRate);
        }
    }
}
