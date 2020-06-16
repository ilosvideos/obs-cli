using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Objects;
using obs_cli.Services;
using obs_cli.Windows;
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace obs_cli.Commands.Implementations
{
    public class GetWebcamDevices : BaseCommand
    {
        private bool WindowWasCreatedHere { get; set; }

        public override string Name => AvailableCommand.GetWebcamDevices.GetDescription();

        public GetWebcamDevices(IDictionary<string, string> arguments)
        {

        }

        public override void Execute()
        {
            if (Store.Data.Webcam.Window == null)
            {
                Store.Data.Webcam.Window = new WebcamWindow();
                Store.Data.Webcam.Window.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
                WindowWasCreatedHere = true;
            }

            if (WindowWasCreatedHere)
            {
                Store.Data.Webcam.Window.EnumerateAndSetWebcams();

                EmitService.EmitWebcamDevices(new WebcamDeviceList
                {
                    Devices = Store.Data.Webcam.Webcams
                });
    
                Store.Data.Webcam.Window = null;                
            }
            else
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

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            EmitService.EmitException(AvailableCommand.GetWebcamDevices.GetDescription(), e.Exception.Message, e.Exception.StackTrace);
        }
    }
}
