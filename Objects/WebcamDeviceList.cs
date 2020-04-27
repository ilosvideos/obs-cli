using System.Collections.Generic;

namespace obs_cli.Objects
{
    public class WebcamDeviceList
    {
        public List<Webcam> Devices { get; set; }

        public WebcamDeviceList()
        {
            Devices = new List<Webcam>();
        }

        public void Add(Webcam device)
        {
            this.Devices.Add(device);
        }
    }
}
