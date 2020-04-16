using obs_cli.Enums;
using obs_cli.Objects;
using obs_cli.Services;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class GetAudioOutputDevices : BaseCommand
    {
        public override string Name => AvailableCommand.GetAudioOutputDevices.GetDescription();

        public GetAudioOutputDevices(IDictionary<string, string> arguments)
        {

        }

        public override void Execute()
        {
            var obsAudioDevices = AudioService.GetAudioOutputDevices();

            var audioDeviceList = new AudioDeviceList();
            obsAudioDevices.ForEach(x => audioDeviceList.Add(x));

            EmitService.EmitAudioOutputDevices(audioDeviceList);
        }
    }
}
