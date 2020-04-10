using System.ComponentModel;

namespace obs_cli.Commands
{
    public enum AvailableCommand
    {
        [Description("initialize")]
        Initialize,
        [Description("pause-recording")]
        PauseRecording,
        [Description("resume-recording")]
        ResumeRecording,
        [Description("start-recording")]
        StartRecording,
        [Description("stop-recording")]
        StopRecording,
        [Description("terminate")]
        Terminate,
    }
}
