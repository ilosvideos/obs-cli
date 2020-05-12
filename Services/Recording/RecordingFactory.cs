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
                service = new WebcamOnlyRecordingService(baseRecordingParameters);
            }
            else
            {
                service = new MainRecordingService(baseRecordingParameters);
            }

            return service;
        }
    }
}
