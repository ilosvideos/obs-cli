using obs_cli.Commands.Implementations;
using obs_cli.Enums;
using System;
using System.Collections.Generic;

namespace obs_cli.Commands
{
    public static class AvailableCommandLookup
    {
        public static Dictionary<string, Type> All = new Dictionary<string, Type>()
        {
            { AvailableCommand.CancelRecording.GetDescription(), typeof(CancelRecording) },
            { AvailableCommand.CaptureMouseClick.GetDescription(), typeof(CaptureMouseClick) },
            { AvailableCommand.ChangeWebcam.GetDescription(), typeof(ChangeWebcam) },
            { AvailableCommand.DeleteLastSection.GetDescription(), typeof(DeleteLastSection) },
            { AvailableCommand.DisableWebcam.GetDescription(), typeof(DisableWebcam) },
            { AvailableCommand.DisableWebcamOnly.GetDescription(), typeof(DisableWebcamOnly) },
            { AvailableCommand.EnableWebcam.GetDescription(), typeof(EnableWebcam) },
            { AvailableCommand.EnableWebcamOnly.GetDescription(), typeof(EnableWebcamOnly) },
            { AvailableCommand.GetAudioInputDevices.GetDescription(), typeof(GetAudioInputDevices) },
            { AvailableCommand.GetAudioOutputDevices.GetDescription(), typeof(GetAudioOutputDevices) },
            { AvailableCommand.GetWebcamDevices.GetDescription(), typeof(GetWebcamDevices) },
            { AvailableCommand.GetWebcamWindowProperties.GetDescription(), typeof(GetWebcamWindowProperties) },
            { AvailableCommand.Initialize.GetDescription(), typeof(Initialize) },
            { AvailableCommand.MoveWebcamWindow.GetDescription(), typeof(MoveWebcamWindow) },
            { AvailableCommand.MouseClickComplete.GetDescription(), typeof(MouseClickComplete) },
            { AvailableCommand.PauseRecording.GetDescription(), typeof(PauseRecording) },
            { AvailableCommand.PositionWebcamWindow.GetDescription(), typeof(PositionWebcamWindow) },
            { AvailableCommand.ResumeRecording.GetDescription(), typeof(ResumeRecording) },
            { AvailableCommand.SetMouseClickHighlightPosition.GetDescription(), typeof(SetMouseClickHighlightPosition) },
            { AvailableCommand.StartRecording.GetDescription(), typeof(StartRecording) },
            { AvailableCommand.StopRecording.GetDescription(), typeof(StopRecording) },
            { AvailableCommand.ToggleFullScreen.GetDescription(), typeof(ToggleFullScreen) },
            { AvailableCommand.UpdateAudioInputDevice.GetDescription(), typeof(UpdateAudioInputDevice) },
            { AvailableCommand.UpdateAudioOutputDevice.GetDescription(), typeof(UpdateAudioOutputDevice) },
            { AvailableCommand.UpdateRecordingAreaPosition.GetDescription(), typeof(UpdateRecordingAreaPosition) }
        };
    }
}
