using obs_cli.Structs;
using System.Collections.Generic;

namespace obs_cli.Objects
{
    public class AudioDeviceList
    {
        public List<AudioDevice> Devices { get; set; }

        public AudioDeviceList()
        {
            Devices = new List<AudioDevice>();
        }

        public void Add(AudioDevice device)
        {
            this.Devices.Add(device);
        }
    }
}
