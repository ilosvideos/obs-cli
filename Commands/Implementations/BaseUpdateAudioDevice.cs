using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public abstract class BaseUpdateAudioDevice : BaseCommand
    {
        public string DeviceId { get; set; }

        protected BaseUpdateAudioDevice(IDictionary<string, string> arguments)
        {
            DeviceId = arguments["deviceId"];
        }
    }
}
