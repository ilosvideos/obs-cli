using obs_cli.Data;
using obs_cli.Enums;
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
            if (!Store.Data.Webcam.IsWebcamEnabled || Store.Data.Webcam.Window == null)
            {
                return;
            }

            Store.Data.Webcam.Window.Dispatcher.Invoke(new Action(() =>
            {
                Store.Data.Webcam.Window.Close();
            }));

            Store.Data.Webcam.IsWebcamOnly = false;
            Store.Data.Webcam.ActiveWebcamValue = string.Empty;
            Store.Data.Webcam.IsWebcamEnabled = false;
        }
    }
}
