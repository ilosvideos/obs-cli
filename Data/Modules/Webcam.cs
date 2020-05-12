using obs_cli.Objects.Obs;
using obs_cli.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using WebcamDevice = obs_cli.Objects.Webcam;

namespace obs_cli.Data.Modules
{
    public class Webcam
    {
        public string ActiveWebcamValue { get; set; }

        public WebcamDevice DefaultWebcam
        {
            get
            {
                return Webcams.FirstOrDefault();
            }
        }

        public bool IsWebcamEnabled { get; set; }

        public bool IsWebcamOnly { get; set; }

        public Item Item { get; set; }

        public Source Source { get; set; }

        public WebcamWindow Window { get; set; }

        public List<WebcamDevice> Webcams { get; set; }

        public IntPtr WindowHandle { get; set; }

        public int WindowMouseX { get; set; }

        public int WindowMouseY { get; set; }

        public Webcam()
        {
            Webcams = new List<WebcamDevice>();
        }
    }
}
