using obs_cli.Data;
using obs_cli.Enums;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class MouseClickComplete : BaseCommand
    {
        public override string Name => AvailableCommand.MouseClickComplete.GetDescription();

        public MouseClickComplete(IDictionary<string, string> arguments)
        {

        }

        public override void Execute()
        {
            if (Store.Data.Obs.ActiveClick != null)
            {
                Store.Data.Obs.ActiveClick.IsLeftButtonDown = false;
                Store.Data.Obs.ActiveClick = null;
            }
        }
    }
}
