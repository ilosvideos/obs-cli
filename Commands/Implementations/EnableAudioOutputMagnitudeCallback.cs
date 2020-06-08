using obs_cli.Enums;
using obs_cli.Services;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class EnableAudioOutputMagnitudeCallback : BaseCommand
    {
        public override string Name => AvailableCommand.EnableAudioOutputMagnitudeCallback.GetDescription();

        public EnableAudioOutputMagnitudeCallback(IDictionary<string, string> arguments)
        {

        }

        public override void Execute()
        {
            AudioService.EnableOutputMagnitudeEmitCallback();
        }
    }
}
