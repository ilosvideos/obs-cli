using System.ComponentModel;

namespace obs_cli.Enums
{
    public enum AvailableCommand
    {
        [Description("cancel-recording")]
        CancelRecording,
        [Description("delete-last-section")]
        DeleteLastSection,
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
