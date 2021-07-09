using obs_cli.Objects;
using obs_cli.Services.Recording.Abstract;
using obs_cli.Services.Recording.Objects;

namespace obs_cli.Services.Recording
{
    public static class RecordingFactory
    {
        public static IBaseRecordingService Make(bool isWebcamOnly, BaseRecordingParameters baseRecordingParameters)
        {
            IBaseRecordingService service;
            if (isWebcamOnly)
            {
                Loggers.CliLogger.Trace("Webcam only. Creating WebcamOnlyRecordingService");
                service = new WebcamOnlyRecordingService(baseRecordingParameters);
            }
            else
            {
                Loggers.CliLogger.Trace("Creating MainRecordingService");
                service = new MainRecordingService(baseRecordingParameters);
            }

            return service;
        }
    }
}
