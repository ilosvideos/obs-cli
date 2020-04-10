using System.ComponentModel;

namespace obs_cli.Commands
{
    public enum AvailableCommand
    {
        [Description("resume-recording")]
        ResumeRecording,
        [Description("start-recording")]
        StartRecording,
    }
}
