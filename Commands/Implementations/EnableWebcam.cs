using obs_cli.Commands.Abstract;
using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace obs_cli.Commands.Implementations
{
    public class EnableWebcam : BaseWebcamInitialization
    {
        public double? Height { get; set; }
        public double? Width { get; set; }

        public override string Name => AvailableCommand.EnableWebcam.GetDescription();

        public EnableWebcam(IDictionary<string, string> arguments)
            :base(arguments)
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
        }

        public override void Execute()
        {
            if (WebcamValue == Store.Data.Webcam.ActiveWebcamValue)
            {
                return;
            }

            if (Store.Data.Webcam.Window == null)
            {
                Store.Data.Webcam.Window = new WebcamWindow();
            }

            Store.Data.Webcam.Window.Show(Width, Height);

            var webcam = Store.Data.Webcam.Webcams.FirstOrDefault(x => x.value == WebcamValue);
            Store.Data.Webcam.Window.setWebcam(webcam);

            Store.Data.Webcam.Window.mainBorder.Visibility = Visibility.Visible;
        }
    }
}
