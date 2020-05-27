using obs_cli.Enums;
using obs_cli.Services;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class UpdateAudioInputDevice : BaseUpdateAudioDevice
    {
        public override string Name => AvailableCommand.UpdateAudioInputDevice.GetDescription();

        public UpdateAudioInputDevice(IDictionary<string, string> arguments)
            : base(arguments) { }

        public override void Execute()
        {
            AudioService.UpdateAudioInput(DeviceId);
            WebcamService.UpdateAudioDevice();
        }
    }
}
