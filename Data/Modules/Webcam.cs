using obs_cli.Objects.Obs;
using obs_cli.Windows;
using System.Collections.Generic;
using WebcamDevice = obs_cli.Objects.Webcam;

namespace obs_cli.Data.Modules
{
    public class Webcam
    {
        public Item Item { get; set; }

        public Source Source { get; set; }

        public WebcamWindow Window { get; set; }

        public List<WebcamDevice> Webcams { get; set; }

        public Webcam()
        {
            Webcams = new List<WebcamDevice>();
        }

        public List<WebcamDevice> GetWebcams()
        {
            if (Window == null)
            {
                Window = new WebcamWindow();
            }

            return Webcams;
        }
    }
}
