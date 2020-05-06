using obs_cli.Commands.Abstract;
using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace obs_cli.Commands.Implementations
{
    public class EnableWebcamOnly : BaseWebcamInitialization
    {
        public double? Height { get; set; }
        public double? Width { get; set; }
        public double? Left { get; set; }
        public double? Top { get; set; }

        public override string Name => AvailableCommand.EnableWebcam.GetDescription();

        public EnableWebcamOnly(IDictionary<string, string> arguments)
            : base(arguments)
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
        }

        public override void Execute()
        {
            if (WebcamValue == Store.Data.Webcam.ActiveWebcamValue)
            {
                return;
            }

            // if no left/top, show center screen
            if (Store.Data.Webcam.Window == null)
            {
                Thread thread = new Thread(() =>
                {
                    Store.Data.Webcam.Window = new WebcamWindow();
                    Store.Data.Webcam.Window.Left = Left.Value;
                    Store.Data.Webcam.Window.Top = Top.Value;
                    Store.Data.Webcam.Window.Show(Width, Height);
                    Store.Data.App.ApplicationInstance = new Application();

                    var webcam = Store.Data.Webcam.Webcams.FirstOrDefault(x => x.value == WebcamValue);
                    Store.Data.Webcam.Window.setWebcam(webcam);

                    Store.Data.Webcam.Window.mainBorder.Visibility = Visibility.Visible;

                    Store.Data.App.ApplicationInstance.Run(Store.Data.Webcam.Window);
                });

                thread.Name = "Webcam Window";
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
            else
            {
                // webcam window is already created
                // is webcam enabled? if so, just move it to the Left/Top position passed in
                // if not, enable the webcam, then move it to the Left/Top position passed in. use first webcam in the list
                Store.Data.Webcam.Window.Dispatcher.Invoke(new Action(() =>
                {
                    Store.Data.Webcam.Window.Left = Left.Value;
                    Store.Data.Webcam.Window.Top = Top.Value;
                    Store.Data.Webcam.Window.Show(Width, Height);

                    var webcam = Store.Data.Webcam.Webcams.FirstOrDefault(x => x.value == WebcamValue);
                    Store.Data.Webcam.Window.setWebcam(webcam);

                    Store.Data.Webcam.Window.mainBorder.Visibility = Visibility.Visible;
                }));
            }
        }
    }
}
