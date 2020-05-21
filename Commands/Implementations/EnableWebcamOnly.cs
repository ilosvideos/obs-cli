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
    public class EnableWebcamOnly : BaseCommand
    {
        public double Height { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width{ get; set; }

        public override string Name => AvailableCommand.EnableWebcamOnly.GetDescription();

        public EnableWebcamOnly(IDictionary<string, string> arguments)
        {
            Left = double.Parse(arguments["left"]);
            Top = double.Parse(arguments["top"]);
            Height = double.Parse(arguments["height"]);
            Width = double.Parse(arguments["width"]);
        }

        public override void Execute()
        {
            try
            {
                // webcam window is not created
                // create webcam window and run it under application
                // show webcam window with first camera
                // position it to the Top/Left values passed in
                if (Store.Data.Webcam.Window == null)
                {
                    Thread thread = new Thread(() =>
                    {
                        Store.Data.Webcam.Window = new WebcamWindow();
                        Store.Data.App.ApplicationInstance = new Application();

                        Store.Data.Webcam.Window.Left = Left;
                        Store.Data.Webcam.Window.Top = Top;
                        Store.Data.Webcam.Window.Show(Width, Height);

                        Store.Data.Webcam.Window.SetWebcam(Store.Data.Webcam.DefaultWebcam);

                        Store.Data.Webcam.Window.mainBorder.Visibility = Visibility.Visible;

                        EmitService.EmitEnableWebcamOnlyResponse(Store.Data.Webcam.Window.selectedWebcam.value, true);
                        Store.Data.App.ApplicationInstance.Run(Store.Data.Webcam.Window);
                    });

                    thread.Name = "Webcam Window";
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                }
                else
                {
                    // webcam window is already created
                    // is webcam enabled? if so, just move it to the Left/Top position passed in. continue to show same webcam
                    // if not, enable the webcam, then move it to the Left/Top position passed in. use first webcam in the list
                    Store.Data.Webcam.Window.Dispatcher.Invoke(new Action(() =>
                    {
                        if (Store.Data.Webcam.IsWebcamEnabled)
                        {
                            Store.Data.Webcam.Window.Left = Left;
                            Store.Data.Webcam.Window.Top = Top;
                            Store.Data.Webcam.Window.Height = Height;
                            Store.Data.Webcam.Window.Width = Width;
                        }
                        else
                        {
                            Store.Data.Webcam.Window.Left = Left;
                            Store.Data.Webcam.Window.Top = Top;
                            Store.Data.Webcam.Window.Show(Width, Height);

                            Store.Data.Webcam.Window.SetWebcam(Store.Data.Webcam.DefaultWebcam);

                            Store.Data.Webcam.Window.mainBorder.Visibility = Visibility.Visible;
                        }

                        EmitService.EmitEnableWebcamOnlyResponse(Store.Data.Webcam.Window.selectedWebcam.value, true);
                    }));
                }

                Store.Data.Webcam.IsWebcamEnabled = true;
                Store.Data.Webcam.IsWebcamOnly = true;
            }
            catch(Exception ex)
            {
                EmitService.EmitEnableWebcamOnlyResponse(ex.Message, false, Constants.Webcam.ErrorMessages.EnableWebcamOnlyFailed);
            }
        }
    }
}
