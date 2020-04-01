using obs_cli.Objects;
using static OBS.libobs;

namespace obs_cli.Data.Modules
{
    public class Obs
    {
        public Presentation Presentation { get; set; }
        public Scene MainScene { get; set; }
        public Scene WebcamScene { get; set; }
        public obs_sceneitem_crop AppliedCrop { get; set; }

        public ObsOutputAndEncoders OutputAndEncoders { get; set; }
    }
}
