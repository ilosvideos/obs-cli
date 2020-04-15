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
            { AvailableCommand.DeleteLastSection.GetDescription(), typeof(DeleteLastSection) },
            { AvailableCommand.GetAudioDevices.GetDescription(), typeof(GetAudioDevices) },
            { AvailableCommand.Initialize.GetDescription(), typeof(Initialize) },
            { AvailableCommand.PauseRecording.GetDescription(), typeof(PauseRecording) },
            { AvailableCommand.ResumeRecording.GetDescription(), typeof(ResumeRecording) },
            { AvailableCommand.StartRecording.GetDescription(), typeof(StartRecording) },
            { AvailableCommand.StopRecording.GetDescription(), typeof(StopRecording) },
            { AvailableCommand.Terminate.GetDescription(), typeof(Terminate) }
        };
    }
}
