using obs_cli.Enums;
using obs_cli.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace obs_cli.Services
{
    // todo: EmitService? ResponseService? OutputService sounds like it's specifically for audio-related things
    public static class OutputService
    {
        /// <summary>
        /// Emits the audio output magnitude level to standard output.
        /// </summary>
        /// <param name="magnitude"></param>
        public static void EmitOutputMagnitude(AudioOutputParameters parameters)
        {
            EmitOutput(OutputMessage.AudioOutputMagnitude, parameters.ToDictionary());
        }

        private static void EmitOutput(OutputMessage messageType, IDictionary<string, string> additionalParameters = null)
        {
            var commandToExecute = new StringBuilder(messageType.GetDescription());

            if (additionalParameters != null)
            {
                StringBuilder parameterString = additionalParameters.Aggregate(new StringBuilder(), (stringBuilder, x) => stringBuilder.Append($" --{x.Key}={x.Value}"));
                commandToExecute.Append(parameterString);
            }

            Console.WriteLine(commandToExecute.ToString());
        }
    }
}
