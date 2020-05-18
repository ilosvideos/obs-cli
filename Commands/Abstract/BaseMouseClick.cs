using System.Collections.Generic;

namespace obs_cli.Commands.Abstract
{
    public abstract class BaseMouseClick : BaseCommand
    {
        public int X { get; set; }
        public int Y { get; set; }

        public BaseMouseClick(IDictionary<string, string> arguments)
        {
            X = int.Parse(arguments["x"]);
            Y = int.Parse(arguments["y"]);
        }
    }
}
