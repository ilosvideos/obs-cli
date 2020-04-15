using obs_cli.Enums;
using obs_cli.Services;
using System;
using System.Collections.Generic;
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

        public void Add(AudioDevice device)
        {
            this.Devices.Add(device);
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
            var obsAudioDevices = AudioService.GetAudioInputDevices();

            var audioDeviceList = new AudioDeviceList();
            obsAudioDevices.ForEach(x => audioDeviceList.Add(new AudioDevice() { Id = x.id, Name = x.name }));

            EmitService.EmitAudioDevices(audioDeviceList);
        }
    }
}
