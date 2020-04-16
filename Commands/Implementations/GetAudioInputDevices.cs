using obs_cli.Enums;
using obs_cli.Objects;
using obs_cli.Services;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class GetAudioInputDevices : BaseCommand
    {
        public override string Name => AvailableCommand.GetAudioInputDevices.GetDescription();

        public GetAudioInputDevices(IDictionary<string, string> arguments)
        {

        }

        public override void Execute()
        {
            var obsAudioDevices = AudioService.GetAudioInputDevices();

            var audioDeviceList = new AudioDeviceList();
            obsAudioDevices.ForEach(x => audioDeviceList.Add(x));

            EmitService.EmitAudioDevices(audioDeviceList);
        }
    }
}
