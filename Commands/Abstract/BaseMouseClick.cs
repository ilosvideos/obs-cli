using obs_cli.Data;
using System.Collections.Generic;
using static OBS.libobs;

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

            if (!Store.Data.Record.AppliedCrop.Equals(default(obs_sceneitem_crop)))
            {
                X = X - Store.Data.Record.AppliedCrop.left;
                Y = Y - Store.Data.Record.AppliedCrop.top;
            }
        }
    }
}
