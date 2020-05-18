using obs_cli.Objects.Mouse.Objects;
using obs_cli.Objects.Obs;
using System.Collections.Generic;
using System.Linq;

namespace obs_cli.Data.Modules
{
    public class Obs
    {
        public Click ActiveClick { get; set; }
        public List<Click> Clicks { get; set; }
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

        public Obs()
        {
            Clicks = new List<Click>();
        }
    }
}
