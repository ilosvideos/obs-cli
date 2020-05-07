using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Services;
using System;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class DisableWebcam : BaseCommand
    {
        public override string Name => AvailableCommand.DisableWebcam.GetDescription();

        public DisableWebcam(IDictionary<string, string> arguments)
        {
            
        }

        public override void Execute()
        {
            Store.Data.Webcam.Window.Dispatcher.Invoke(new Action(() =>
            {
                Store.Data.Webcam.Window.Close();
            }));

            if (Store.Data.Webcam.IsWebcamOnly)
            {
                Store.Data.Obs.WebcamScene.ClearItems();
                Store.Data.Obs.Presentation.SetScene(Store.Data.Obs.MainScene);
            }

            Store.Data.Webcam.IsWebcamOnly = false;
            Store.Data.Webcam.ActiveWebcamValue = string.Empty;
            Store.Data.Webcam.IsWebcamEnabled = false;
        }
    }
}
