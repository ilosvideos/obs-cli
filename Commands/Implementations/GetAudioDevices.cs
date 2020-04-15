using Newtonsoft.Json;
using obs_cli.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;

namespace obs_cli.Commands.Implementations
{
    public class AudioDevice
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class AudioDeviceList
    {
        public List<AudioDevice> Devices { get; set; }

        public AudioDeviceList()
        {
            Devices = new List<AudioDevice>();
        }
    }

    public class GetAudioDevices : BaseCommand
    {
        public override string Name => AvailableCommand.GetAudioDevices.GetDescription();

        public GetAudioDevices(IDictionary<string, string> arguments)
        {

        }

        public override void Execute()
        {
            var test = new AudioDeviceList();
            test.Devices.Add(new AudioDevice() { Id = Guid.NewGuid().ToString(), Name = "Test1" });
            test.Devices.Add(new AudioDevice() { Id = Guid.NewGuid().ToString(), Name = "Test2" });
            test.Devices.Add(new AudioDevice() { Id = Guid.NewGuid().ToString(), Name = "Test3" });
            test.Devices.Add(new AudioDevice() { Id = Guid.NewGuid().ToString(), Name = "Test4" });

            var serializedString = new JavaScriptSerializer().Serialize(test);

            Console.WriteLine($"get-audio-devices --response={serializedString}");
        }
    }
}
