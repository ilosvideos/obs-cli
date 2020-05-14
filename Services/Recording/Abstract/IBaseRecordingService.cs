namespace obs_cli.Services.Recording.Abstract
{
    public interface IBaseRecordingService
    {
        void Setup();
        bool StartRecording();
    }
}
