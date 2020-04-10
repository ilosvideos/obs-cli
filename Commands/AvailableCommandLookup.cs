using obs_cli.Commands.Implementations;
using System;
using System.Collections.Generic;

namespace obs_cli.Commands
{
    public static class AvailableCommandLookup
    {
        public static Dictionary<string, Type> All = new Dictionary<string, Type>()
        {
            { AvailableCommand.CancelRecording.GetDescription(), typeof(CancelRecording) },
            { AvailableCommand.Initialize.GetDescription(), typeof(Initialize) },
            { AvailableCommand.StartRecording.GetDescription(), typeof(StartRecording) },
            { AvailableCommand.StopRecording.GetDescription(), typeof(StopRecording) },
            { AvailableCommand.PauseRecording.GetDescription(), typeof(PauseRecording) },
            { AvailableCommand.ResumeRecording.GetDescription(), typeof(ResumeRecording) },
            { AvailableCommand.Terminate.GetDescription(), typeof(Terminate) }
        };
    }
}
