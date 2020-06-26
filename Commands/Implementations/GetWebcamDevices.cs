using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Objects;
using obs_cli.Services;
using System;
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
            Store.Data.Webcam.Window.Dispatcher.Invoke(new Action(() =>
            {
                Store.Data.Webcam.Window.EnumerateAndSetWebcams();

                EmitService.EmitWebcamDevices(new WebcamDeviceList
                {
                    Devices = Store.Data.Webcam.Webcams
                });
            }));
        }
    }
}
