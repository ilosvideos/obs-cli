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
    public class EnableWebcam : BaseWebcamInitialization
    {
        public double? Height { get; set; }
        public double? Width { get; set; }
        public double? Left { get; set; }
        public double? Top { get; set; }

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

            var showWebcam = new Action(() => 
            {
                Store.Data.Webcam.Window.Left = Left.Value;
                Store.Data.Webcam.Window.Top = Top.Value;
                Store.Data.Webcam.Window.Show(Width, Height);

                var webcam = Store.Data.Webcam.GetWebcam(WebcamValue);
                Store.Data.Webcam.Window.setWebcam(webcam);

                Store.Data.Webcam.Window.mainBorder.Visibility = Visibility.Visible;
            });

            // todo: if no left/top, show center screen
            if (Store.Data.Webcam.Window == null)
            {
                Thread thread = new Thread(() =>
                {
                    Store.Data.Webcam.Window = new WebcamWindow();
                    Store.Data.App.ApplicationInstance = new Application();

                    showWebcam();

                    Store.Data.App.ApplicationInstance.Run(Store.Data.Webcam.Window);
                });

                thread.Name = "Webcam Window";
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
            else
            {
                Store.Data.Webcam.Window.Dispatcher.Invoke(new Action(() =>
                {
                    showWebcam();
                }));
            }

            Store.Data.Webcam.IsWebcamEnabled = true;
        }
    }
}
