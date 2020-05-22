using OBS;

namespace obs_cli.Data.Modules
{
    public class WebcamSettings
    {
        public bool ShouldUseCustomSettings { get; set; }
        public int Fps { get; set; }
        public string Resolution { get; set; }
        public videoformat VideoFormat { get; set; }
    }
}
