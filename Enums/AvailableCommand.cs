using System.ComponentModel;

namespace obs_cli.Enums
{
    public enum AvailableCommand
    {
        [Description("audio-input-magnitude")]
        AudioInputMagnitude,
        [Description("audio-output-magnitude")]
        AudioOutputMagnitude,
        [Description("cancel-recording")]
        CancelRecording,
        [Description("delete-last-section")]
        DeleteLastSection,
        [Description("get-audio-input-devices")]
        GetAudioInputDevices,
        [Description("get-audio-output-devices")]
        GetAudioOutputDevices,
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
