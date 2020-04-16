using obs_cli.Enums;
using obs_cli.Services;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class UpdateAudioOutputDevice : BaseUpdateAudioDevice
    {
        public override string Name => AvailableCommand.UpdateAudioOutputDevice.GetDescription();

        public UpdateAudioOutputDevice(IDictionary<string, string> arguments)
            : base(arguments) { }

        public override void Execute()
        {
            AudioService.UpdateAudioOutput(DeviceId);
        }
    }
}
