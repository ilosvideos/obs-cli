using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Objects;
using obs_cli.Services;
using System;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class GetWebcamWindowProperties : BaseCommand
    {
        public override string Name => AvailableCommand.GetWebcamWindowProperties.GetDescription();

        public GetWebcamWindowProperties(IDictionary<string, string> arguments)
        {

        }

        public override void Execute()
        {
            var properties = new WebcamWindowProperties()
            {
                IsEnabled = Store.Data.Webcam.IsWebcamEnabled
            };

            if (Store.Data.Webcam.IsWebcamEnabled && Store.Data.Webcam.Window != null)
            {
                Store.Data.Webcam.Window.Dispatcher.Invoke(new Action(() =>
                {
                    properties.Left = Store.Data.Webcam.Window.Left;
                    properties.Top = Store.Data.Webcam.Window.Top;
                    properties.Height = Store.Data.Webcam.Window.Height;
                    properties.Width = Store.Data.Webcam.Window.Width;
                    properties.BorderSize = Store.Data.Webcam.Window.BorderSize;
                }));
            }

            EmitService.EmitWebcamWindowProperties(properties);
        }
    }
}
