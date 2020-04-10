using obs_cli.Commands.Implementations;
using System;
using System.Collections.Generic;

namespace obs_cli.Commands
{
    public static class AvailableCommands
    {
        public static Dictionary<string, Type> All = new Dictionary<string, Type>()
        {
            { Initialize.Name, typeof(Initialize) },
            { StartRecording.Name, typeof(StartRecording) },
            { StopRecording.Name, typeof(StopRecording) },
            { PauseRecording.Name, typeof(PauseRecording) },
            { Terminate.Name, typeof(Terminate) }
        };
    }
}
