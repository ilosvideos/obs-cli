using obs_cli.Commands.Abstract;
using obs_cli.Data;
using obs_cli.Enums;
using System;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class PositionWebcamWindow : TranslateWebcamWindow
    {
        public override string Name => AvailableCommand.PositionWebcamWindow.GetDescription();

        public PositionWebcamWindow(IDictionary<string, string> arguments)
            : base(arguments) { }

        public override void Translate()
        {
            Store.Data.Webcam.Window.Top = Top;
            Store.Data.Webcam.Window.Left = Left;
        }
    }
}
