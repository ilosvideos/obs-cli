using obs_cli.Commands.Abstract;
using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Services;
using System;
using System.Collections.Generic;
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
                Store.Data.Webcam.Window.Dispatcher.Invoke(new Action(() =>
                {
                    Store.Data.Webcam.Window.Left = Left.Value;
                    Store.Data.Webcam.Window.Top = Top.Value;
                    Store.Data.Webcam.Window.Show(Width, Height);

                    if (!Store.Data.Webcam.IsWebcamEnabled)
                    {
                        Store.Data.Webcam.Window.SetWebcam(Store.Data.Webcam.DefaultWebcam);
                        Store.Data.Webcam.Window.mainBorder.Visibility = Visibility.Visible;
                    }

                    EmitService.EmitEnableWebcamResponse(AvailableCommand.EnableWebcamOnly, Store.Data.Webcam.Window.selectedWebcam.value, true);
                }));

                Store.Data.Webcam.IsWebcamEnabled = true;
                Store.Data.Webcam.IsWebcamOnly = true;
            }
            catch (Exception ex)
            {
                //EmitService.EmitEnableWebcamResponse(AvailableCommand.EnableWebcamOnly, ex.Message, false, Constants.Webcam.ErrorMessages.EnableWebcamOnlyFailed);
                throw ex;
            }
        }
    }
}
