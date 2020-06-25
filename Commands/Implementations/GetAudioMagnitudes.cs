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
                IsSuccessful = true
            };

            if (Store.Data.Audio.InputLevel.HasValue)
            {
                response.AudioInputLevel = Store.Data.Audio.InputLevel.Value;
            }

            if (Store.Data.Audio.OutputLevel.HasValue)
            {
                response.AudioInputLevel = Store.Data.Audio.OutputLevel.Value;
            }

            EmitService.EmitAudioMagnitudes(response);
        }
    }
}
