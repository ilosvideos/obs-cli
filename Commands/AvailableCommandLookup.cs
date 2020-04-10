using obs_cli.Commands.Implementations;
using System;
using System.Collections.Generic;

namespace obs_cli.Commands
{
    public static class AvailableCommandLookup
    {
        public static Dictionary<string, Type> All = new Dictionary<string, Type>()
        {
            { Initialize.Name, typeof(Initialize) },
            { AvailableCommand.StartRecording.GetDescription(), typeof(StartRecording) },
            { StopRecording.Name, typeof(StopRecording) },
            { PauseRecording.Name, typeof(PauseRecording) },
            { ResumeRecording.Name, typeof(ResumeRecording) },
            { Terminate.Name, typeof(Terminate) }
        };
    }
}
