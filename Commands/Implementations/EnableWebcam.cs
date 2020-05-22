using obs_cli.Commands.Abstract;
using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Services;
using obs_cli.Utility;
using obs_cli.Windows;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace obs_cli.Commands.Implementations
{
    public class EnableWebcam : EnableWebcamInitialization
    {
        public override string Name => AvailableCommand.EnableWebcam.GetDescription();

        public EnableWebcam(IDictionary<string, string> arguments)
            :base(arguments) { }

        public override void Execute()
        {
            if (WebcamValue == Store.Data.Webcam.ActiveWebcamValue)
            {
                return;
            }

            var showWebcam = new Action(() => 
            {
                if (!Left.HasValue || !Top.HasValue)
                {
                    Store.Data.Webcam.Window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
                else
                {
                    Store.Data.Webcam.Window.Left = Left.Value;
                    Store.Data.Webcam.Window.Top = Top.Value;
                }
                
                Store.Data.Webcam.Window.Show(Width, Height);

                var webcam = WebcamService.GetWebcam(WebcamValue);

                if (webcam == null)
                {
                    throw new Exception(Constants.Webcam.ErrorMessages.WebcamNotFound);
                }

                Store.Data.Webcam.Window.SetWebcam(webcam);

                Store.Data.Webcam.Window.mainBorder.Visibility = Visibility.Visible;
                EmitService.EmitEnableWebcamResponse(AvailableCommand.EnableWebcam, Store.Data.Webcam.Window.selectedWebcam.value, true);
            });

            if (Store.Data.Webcam.Window == null)
            {
                Thread thread = new Thread(() =>
                {
                    Store.Data.Webcam.Window = new WebcamWindow();
                    Store.Data.App.ApplicationInstance = new Application();

                    showWebcam();

                    Store.Data.App.ApplicationInstance.Run(Store.Data.Webcam.Window);
                });

                thread.Name = Constants.Webcam.Settings.WebcamWindowThreadName;
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
