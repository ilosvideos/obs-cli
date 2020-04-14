using System.ComponentModel;

namespace obs_cli.Enums
{
    public enum OutputMessage
    {
        [Description("audio-input-magnitude")]
        AudioInputMagnitude,
        [Description("audio-output-magnitude")]
        AudioOutputMagnitude
    }
}
