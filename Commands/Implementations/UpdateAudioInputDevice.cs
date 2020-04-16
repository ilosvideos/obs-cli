using obs_cli.Enums;
using obs_cli.Services;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class UpdateAudioInputDevice : BaseCommand
    {
        public override string Name => AvailableCommand.UpdateAudioInputDevice.GetDescription();

        public string DeviceId { get; set; }

        public UpdateAudioInputDevice(IDictionary<string, string> arguments)
        {
            DeviceId = arguments["deviceId"];
        }

        public override void Execute()
        {
            AudioService.UpdateAudioInput(DeviceId);
        }
    }
}
