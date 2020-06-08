using obs_cli.Enums;
using obs_cli.Services;
using obs_cli.Utility;
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

            if (!string.IsNullOrWhiteSpace(DeviceId) && DeviceId != Constants.Audio.NO_DEVICE_ID)
            {
                if (!AudioService.IsAudioInputCallbackEnabled)
                {
                    AudioService.EnableInputMagnitudeEmitCallback();
                }
            }
            else
            {
                AudioService.DisableInputMagnitudeCallback();
            }
        }
    }
}
