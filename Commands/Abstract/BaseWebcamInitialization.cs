using OBS;
using System.Collections.Generic;

namespace obs_cli.Commands.Abstract
{
    public abstract class BaseWebcamInitialization : BaseCommand
    {
        // todo: this class needs to accept parameters for the WebcamValue's custom hardware whitelist if applicable

        public int Fps { get; set; }
        public string Resolution { get; set; }
        public bool ShouldUseCustomSettings { get; set; }
        public videoformat VideoFormat { get; set; }
        public string WebcamValue { get; set; }

        public double? Height { get; set; }
        public double? Width { get; set; }
        public double? Left { get; set; }
        public double? Top { get; set; }

        protected BaseWebcamInitialization(IDictionary<string, string> arguments)
        {
            double height;
            if (double.TryParse(arguments["height"], out height))
            {
                Height = height;
            }

            double width;
            if (double.TryParse(arguments["width"], out width))
            {
                Width = width;
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

            if (arguments.ContainsKey("webcamValue"))
            {
                WebcamValue = arguments["webcamValue"];
            }
        }
    }
}
