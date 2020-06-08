using obs_cli.Enums;
using obs_cli.Services;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class EnableAudioInputMagnitudeCallback : BaseCommand
    {
        public override string Name => AvailableCommand.EnableAudioInputMagnitudeCallback.GetDescription();

        public EnableAudioInputMagnitudeCallback(IDictionary<string, string> arguments)
        {

        }

        public override void Execute()
        {
            AudioService.EnableInputMagnitudeEmitCallback();
        }
    }
}
