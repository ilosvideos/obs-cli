using obs_cli.Objects.Obs;

namespace obs_cli.Data.Modules
{
    public class Obs
    {
        public Presentation Presentation { get; set; }
        public Scene MainScene { get; set; }
        public Scene WebcamScene { get; set; }
    }
}
