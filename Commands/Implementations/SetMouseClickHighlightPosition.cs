using obs_cli.Commands.Abstract;
using obs_cli.Data;
using obs_cli.Enums;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class SetMouseClickHighlightPosition : BaseMouseClick
    {
        public override string Name => AvailableCommand.SetMouseClickHighlightPosition.GetDescription();

        public SetMouseClickHighlightPosition(IDictionary<string, string> arguments)
            : base(arguments) { }

        public override void Execute()
        {
            if (Store.Data.Obs.ActiveClick != null)
            {
                Store.Data.Obs.ActiveClick.SetHighlightPosition(X, Y);
            }
        }
    }
}
