using obs_cli.Data;
using obs_cli.Enums;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class UpdateRecordingAreaPosition : BaseCommand
    {
        public override string Name => AvailableCommand.UpdateRecordingAreaPosition.GetDescription();

        public double BorderWidth { get; set; }
        // todo: maybe make a generic "Position" abstract class since a few of the webcam window commands also just accept left/top
        public double Left { get; set; }
        public double Top { get; set; }

        public UpdateRecordingAreaPosition(IDictionary<string, string> arguments)
        {
            double borderWidth;
            if (double.TryParse(arguments["borderWidth"], out borderWidth))
            {
                BorderWidth = borderWidth;
            }

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
            Store.Data.SelectionWindow.Top = Top;
            Store.Data.SelectionWindow.Left = Left;
            Store.Data.SelectionWindow.BorderWidth = BorderWidth;
        }
    }
}
