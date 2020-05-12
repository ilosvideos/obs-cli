using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Objects.Obs;
using obs_cli.Services.Recording.Objects;

namespace obs_cli.Services.Recording.Abstract
{
    public abstract class BaseRecordingService : IBaseRecordingService
    {
        public uint FrameRate { get; set; }

        public string VideoOutputFolder { get; set; }

        public BaseRecordingService(BaseRecordingParameters parameters)
        {
            FrameRate = parameters.FrameRate;
            VideoOutputFolder = parameters.VideoOutputFolder;
        }

        public abstract void Setup();

        public void StartRecording()
        {
            Store.Data.Record.VideoOutputFolder = VideoOutputFolder;

            Setup();

            ObsOutputAndEncoders outputAndEncoders = ObsService.CreateNewObsOutput();
            Store.Data.Record.OutputAndEncoders = outputAndEncoders;
            Store.Data.Record.OutputAndEncoders.obsOutput.Start();

            EmitService.EmitStatusResponse(AvailableCommand.StartRecording, true);
        }
    }
}
