using obs_cli.Data;
using System;
using System.Collections.Generic;

namespace obs_cli.Commands.Abstract
{
    public abstract class TranslateWebcamWindow : BaseCommand
    {
        public double Left { get; set; }
        public double Top { get; set; }

        public TranslateWebcamWindow(IDictionary<string, string> arguments)
        {
            double left;
            if (double.TryParse(arguments["left"], out left))
            {
                Left = left;
            }

            double top;
            if (double.TryParse(arguments["top"], out top))
            {
                Top = top;
            }
        }

        public abstract void Translate();

        public override void Execute()
        {
            // todo: figure out how/why this is being called if webcam window isn't initialized yet
            if (Store.Data.Webcam.Window == null)
            {
                return;
            }

            Store.Data.Webcam.Window.Dispatcher.Invoke(new Action(() =>
            {
                Translate();
            }));
        }
    }
}