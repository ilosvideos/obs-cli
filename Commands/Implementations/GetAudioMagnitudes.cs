using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Objects;
using obs_cli.Services;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class GetAudioMagnitudes : BaseCommand
    {
        public override string Name => AvailableCommand.GetAudioMagnitudes.GetDescription();

        public GetAudioMagnitudes(IDictionary<string, string> arguments)
        {

        }

        public override void Execute()
        {
            var response = new AudioMagnitudesResponse
            {
                IsSuccessful = true,
                AudioInputLevel = Store.Data.Audio.InputMeter.Level,
                AudioOutputLevel = Store.Data.Audio.OutputMeter.Level,
            };

            EmitService.EmitAudioMagnitudes(response);
        }
    }
}
