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
    public class EnableWebcamOnly : EnableWebcamInitialization
    {
        public override string Name => AvailableCommand.EnableWebcamOnly.GetDescription();

        public EnableWebcamOnly(IDictionary<string, string> arguments)
            : base(arguments) { }

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

                        Store.Data.Webcam.Window.Left = Left.Value;
                        Store.Data.Webcam.Window.Top = Top.Value;
                        Store.Data.Webcam.Window.Show(Width, Height);

                        Store.Data.Webcam.Window.SetWebcam(Store.Data.Webcam.DefaultWebcam);

                        Store.Data.Webcam.Window.mainBorder.Visibility = Visibility.Visible;

                        EmitService.EmitEnableWebcamResponse(AvailableCommand.EnableWebcamOnly, Store.Data.Webcam.Window.selectedWebcam.value, true);
                        Store.Data.App.ApplicationInstance.Run(Store.Data.Webcam.Window);
                    });

                    thread.Name = Constants.Webcam.Settings.WebcamWindowThreadName;
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
                            Store.Data.Webcam.Window.Left = Left.Value;
                            Store.Data.Webcam.Window.Top = Top.Value;
                            Store.Data.Webcam.Window.Height = Height.Value;
                            Store.Data.Webcam.Window.Width = Width.Value;
                        }
                        else
                        {
                            Store.Data.Webcam.Window.Left = Left.Value;
                            Store.Data.Webcam.Window.Top = Top.Value;
                            Store.Data.Webcam.Window.Show(Width, Height);

                            Store.Data.Webcam.Window.SetWebcam(Store.Data.Webcam.DefaultWebcam);

                            Store.Data.Webcam.Window.mainBorder.Visibility = Visibility.Visible;
                        }

                        EmitService.EmitEnableWebcamResponse(AvailableCommand.EnableWebcamOnly, Store.Data.Webcam.Window.selectedWebcam.value, true);
                    }));
                }

                Store.Data.Webcam.IsWebcamEnabled = true;
                Store.Data.Webcam.IsWebcamOnly = true;
            }
            catch(Exception ex)
            {
                EmitService.EmitEnableWebcamResponse(AvailableCommand.EnableWebcamOnly, ex.Message, false, Constants.Webcam.ErrorMessages.EnableWebcamOnlyFailed);
            }
        }
    }
}
