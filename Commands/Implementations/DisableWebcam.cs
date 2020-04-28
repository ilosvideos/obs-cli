using obs_cli.Data;
using obs_cli.Enums;
using System.Collections.Generic;
using System.Windows;

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
            Store.Data.Webcam.Window.Close();
            Store.Data.Webcam.Window = null;
            Store.Data.Webcam.ActiveWebcamValue = string.Empty;
        }
    }
}
