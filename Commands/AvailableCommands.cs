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
            { Terminate.Name, typeof(Terminate) }
        };
    }
}
