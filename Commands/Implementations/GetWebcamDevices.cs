using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Objects;
using obs_cli.Services;
using obs_cli.Windows;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class GetWebcamDevices : BaseCommand
    {
        public override string Name => AvailableCommand.GetWebcamDevices.GetDescription();

        public GetWebcamDevices(IDictionary<string, string> arguments)
        {

        }

        public override void Execute()
        {
            if (Store.Data.Webcam.Window == null)
            {
                Store.Data.Webcam.Window = new WebcamWindow();
            }

            Store.Data.Webcam.Window.EnumerateAndSetWebcams();

            EmitService.EmitWebcamDevices(new WebcamDeviceList
            {
                Devices = Store.Data.Webcam.Webcams
            });
        }
    }
}
