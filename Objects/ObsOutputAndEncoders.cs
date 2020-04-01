using OBS;

namespace obs_cli.Objects
{
    public class ObsOutputAndEncoders
    {
        public ObsOutput obsOutput;
        public ObsEncoder obsVideoEncoder;
        public ObsEncoder obsAudioEncoder;

        public void Dispose()
        {
            obsOutput?.Dispose();
            obsOutput = null;
            obsVideoEncoder?.Dispose();
            obsVideoEncoder = null;
            obsAudioEncoder?.Dispose();
            obsAudioEncoder = null;
        }
    }
}
