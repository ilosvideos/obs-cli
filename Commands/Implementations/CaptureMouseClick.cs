using obs_cli.Commands.Abstract;
using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Objects.Mouse.Objects;
using System;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class CaptureMouseClick : BaseMouseClick
    {
        public int ScreenBoundsHeight { get; set; }
        public int ScreenBoundsWidth { get; set; }

        public override string Name => AvailableCommand.CaptureMouseClick.GetDescription();

        public CaptureMouseClick(IDictionary<string, string> arguments)
            :base(arguments) 
        {
            ScreenBoundsHeight = int.Parse(arguments["screenBoundsHeight"]);
            ScreenBoundsWidth = int.Parse(arguments["screenBoundsWidth"]);
        }

        public override void Execute()
        {
            var click = new Click(X, Y, ScreenBoundsHeight, ScreenBoundsWidth, RemoveClickFromList);
            Store.Data.Obs.Clicks.Add(click);
            Store.Data.Obs.ActiveClick = click;
        }

        private void RemoveClickFromList(Guid id)
        {
            var click = Store.Data.Obs.Clicks.Find(x => x.Id == id);
            Store.Data.Obs.Clicks.Remove(click);
        }
    }
}
