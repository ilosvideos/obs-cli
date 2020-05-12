using obs_cli.Data;
using obs_cli.Enums;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class ToggleFullScreen : BaseCommand
    {
        public bool IsFullScreen { get; set; }

        public ToggleFullScreen(IDictionary<string, string> arguments)
        {
            IsFullScreen = bool.Parse(arguments["isFullScreen"]);
        }

        public override string Name => AvailableCommand.ToggleFullScreen.GetDescription();

        public override void Execute()
        {
            Store.Data.Record.IsFullScreen = IsFullScreen;
        }
    }
}
