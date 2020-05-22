using OBS;
using obs_cli.Data;
using obs_cli.Utility;
using System;
using System.Collections.Generic;

namespace obs_cli.Commands.Abstract
{
    public abstract class EnableWebcamInitialization : BaseWebcamInitialization
    {
        public int Fps { get; set; }
        public string Resolution { get; set; }
        public bool ShouldUseCustomSettings { get; set; }
        public videoformat VideoFormat { get; set; }

        public double? Height { get; set; }
        public double? Width { get; set; }
        public double? Left { get; set; }
        public double? Top { get; set; }

        public EnableWebcamInitialization(IDictionary<string, string> arguments)
            :base(arguments)
        {
            double height;
            if (double.TryParse(arguments[Constants.Webcam.Parameters.Height], out height))
            {
                Height = height;
            }

            double width;
            if (double.TryParse(arguments[Constants.Webcam.Parameters.Width], out width))
            {
                Width = width;
            }

            double left;
            if (double.TryParse(arguments[Constants.Webcam.Parameters.Left], out left))
            {
                Left = left;
            }

            double top;
            if (double.TryParse(arguments[Constants.Webcam.Parameters.Top], out top))
            {
                Top = top;
            }

            if (arguments.ContainsKey(Constants.Webcam.Parameters.ShouldUseCustomSettings))
            {
                ShouldUseCustomSettings = bool.Parse(arguments[Constants.Webcam.Parameters.ShouldUseCustomSettings]);
                Store.Data.Webcam.WebcamSettings.ShouldUseCustomSettings = ShouldUseCustomSettings;

                if (ShouldUseCustomSettings)
                {
                    Fps = int.Parse(arguments[Constants.Webcam.Parameters.Fps]);
                    Resolution = arguments[Constants.Webcam.Parameters.Resolution];
                    VideoFormat = (videoformat)Enum.Parse(typeof(videoformat), arguments[Constants.Webcam.Parameters.VideoFormat]);

                    Store.Data.Webcam.WebcamSettings.Fps = Fps;
                    Store.Data.Webcam.WebcamSettings.Resolution = Resolution;
                    Store.Data.Webcam.WebcamSettings.VideoFormat = VideoFormat;
                }
            }
        }
    }
}
