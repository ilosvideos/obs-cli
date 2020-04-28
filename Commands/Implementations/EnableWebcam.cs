using obs_cli.Data;
using obs_cli.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace obs_cli.Commands.Implementations
{
    public class EnableWebcam : BaseCommand
    {
        public double? Height { get; set; }
        public double? Width { get; set; }
        public string WebcamValue { get; set; }

        public override string Name => AvailableCommand.EnableWebcam.GetDescription();

        public EnableWebcam(IDictionary<string, string> arguments)
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

            WebcamValue = arguments["webcamValue"];
        }

        public override void Execute()
        {
            Store.Data.Webcam.Window.Show(Width, Height);

            var webcam = Store.Data.Webcam.Webcams.FirstOrDefault(x => x.value == WebcamValue);
            Store.Data.Webcam.Window.setWebcam(webcam);

            Store.Data.Webcam.Window.mainBorder.Visibility = Visibility.Visible;
        }
    }
}
