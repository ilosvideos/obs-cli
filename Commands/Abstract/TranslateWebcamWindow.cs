using System.Collections.Generic;

namespace obs_cli.Commands.Abstract
{
    public abstract class TranslateWebcamWindow : BaseCommand
    {
        public double Left { get; set; }
        public double Top { get; set; }

        public TranslateWebcamWindow(IDictionary<string, string> arguments)
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
    }
}
