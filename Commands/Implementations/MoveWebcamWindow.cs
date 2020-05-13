using obs_cli.Commands.Abstract;
using obs_cli.Data;
using obs_cli.Enums;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class MoveWebcamWindow : TranslateWebcamWindow
    {
        public override string Name => AvailableCommand.MoveWebcamWindow.GetDescription();

        public MoveWebcamWindow(IDictionary<string, string> arguments)
            : base(arguments) { }

        public override void Translate()
        {
            Store.Data.Webcam.Window.Top = Store.Data.Webcam.Window.Top + Top;
            Store.Data.Webcam.Window.Left = Store.Data.Webcam.Window.Left + Left;
        }
    }
}
