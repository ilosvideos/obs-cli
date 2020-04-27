using obs_cli.Windows;
using System.Collections.Generic;
using WebcamDevice = obs_cli.Objects.Webcam;

namespace obs_cli.Data.Modules
{
    public class Webcam
    {
        public WebcamWindow Window { get; set; }

        public List<WebcamDevice> Webcams { get; set; }

        public Webcam()
        {
            Window = new WebcamWindow();
            Webcams = new List<WebcamDevice>();
        }
    }
}
