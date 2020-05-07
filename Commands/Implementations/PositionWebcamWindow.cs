using obs_cli.Data;
using obs_cli.Enums;
using System;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class PositionWebcamWindow : BaseCommand
    {
        public override string Name => AvailableCommand.PositionWebcamWindow.GetDescription();

        public double Left { get; set; }
        public double Top { get; set; }

        public PositionWebcamWindow(IDictionary<string, string> arguments)
        {
            double left;
            if (double.TryParse(arguments["left"], out left))
            {
                Left = left;
            }

            double top;
            if (double.TryParse(arguments["top"], out top))
            {
                Top = top;
            }
        }

        public override void Execute()
        {
            Store.Data.Webcam.Window.Dispatcher.Invoke(new Action(() =>
            {
                Store.Data.Webcam.Window.Top = Top;
                Store.Data.Webcam.Window.Left = Left;
            }));
        }
    }
}
