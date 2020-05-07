using obs_cli.Objects.Obs;
using System.Linq;

namespace obs_cli.Data.Modules
{
    public class Obs
    {
        public Presentation Presentation { get; set; }
        public Scene MainScene 
        {
            get 
            {
                return Presentation.Scenes.Where(x => x.Name == "Main").FirstOrDefault();
            }
        }

        public Scene WebcamScene 
        {
            get
            { 
                return Presentation.Scenes.Where(x => x.Name == "Webcam").FirstOrDefault();
            } 
        }
    }
}
