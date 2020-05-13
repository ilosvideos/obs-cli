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
            { AvailableCommand.ChangeWebcam.GetDescription(), typeof(ChangeWebcam) },
            { AvailableCommand.DeleteLastSection.GetDescription(), typeof(DeleteLastSection) },
            { AvailableCommand.DisableWebcam.GetDescription(), typeof(DisableWebcam) },
            { AvailableCommand.EnableWebcam.GetDescription(), typeof(EnableWebcam) },
            { AvailableCommand.EnableWebcamOnly.GetDescription(), typeof(EnableWebcamOnly) },
            { AvailableCommand.GetAudioInputDevices.GetDescription(), typeof(GetAudioInputDevices) },
            { AvailableCommand.GetAudioOutputDevices.GetDescription(), typeof(GetAudioOutputDevices) },
            { AvailableCommand.GetWebcamDevices.GetDescription(), typeof(GetWebcamDevices) },
            { AvailableCommand.Initialize.GetDescription(), typeof(Initialize) },
            { AvailableCommand.MoveWebcamWindow.GetDescription(), typeof(MoveWebcamWindow) },
            { AvailableCommand.PauseRecording.GetDescription(), typeof(PauseRecording) },
            { AvailableCommand.PositionWebcamWindow.GetDescription(), typeof(PositionWebcamWindow) },
            { AvailableCommand.ResumeRecording.GetDescription(), typeof(ResumeRecording) },
            { AvailableCommand.StartRecording.GetDescription(), typeof(StartRecording) },
            { AvailableCommand.StopRecording.GetDescription(), typeof(StopRecording) },
            { AvailableCommand.Terminate.GetDescription(), typeof(Terminate) },
            { AvailableCommand.ToggleFullScreen.GetDescription(), typeof(ToggleFullScreen) },
            { AvailableCommand.UpdateAudioInputDevice.GetDescription(), typeof(UpdateAudioInputDevice) },
            { AvailableCommand.UpdateAudioOutputDevice.GetDescription(), typeof(UpdateAudioOutputDevice) }
        };
    }
}
