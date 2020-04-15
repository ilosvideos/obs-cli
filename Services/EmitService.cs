using obs_cli.Commands.Implementations;
using obs_cli.Enums;
using obs_cli.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace obs_cli.Services
{
    public static class EmitService
    {
        /// <summary>
        /// Emits the audio input magnitude level to standard output.
        /// </summary>
        /// <param name="magnitude"></param>
        public static void EmitInputMagnitude(AudioMagnitudeParameters parameters)
        {
            EmitOutput(EmitMessage.AudioInputMagnitude, parameters.ToDictionary());
        }

        /// <summary>
        /// Emits the audio output magnitude level to standard output.
        /// </summary>
        /// <param name="magnitude"></param>
        public static void EmitOutputMagnitude(AudioMagnitudeParameters parameters)
        {
            EmitOutput(EmitMessage.AudioOutputMagnitude, parameters.ToDictionary());
        }

        public static void EmitAudioDevices(AudioDeviceList audioDeviceList)
        {
            EmitSerializedOutput(EmitMessage.GetAudioDevices, audioDeviceList);
        }

        /// <summary>
        /// Emits the message type with the given data serialized as JSON.
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="dataToSerialize"></param>
        private static void EmitSerializedOutput(EmitMessage messageType, object dataToSerialize)
        {
            var serializedString = new JavaScriptSerializer().Serialize(dataToSerialize);
            Console.WriteLine($"{ messageType.GetDescription() } --response={ serializedString }");
        }

        private static void EmitOutput(EmitMessage messageType, IDictionary<string, string> additionalParameters = null)
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
