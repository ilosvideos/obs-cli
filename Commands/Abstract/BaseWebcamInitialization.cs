using OBS;
using obs_cli.Utility;
using System;
using System.Collections.Generic;

namespace obs_cli.Commands.Abstract
{
    public abstract class BaseWebcamInitialization : BaseCommand
    {
        public string WebcamValue { get; set; }

        public BaseWebcamInitialization(IDictionary<string, string> arguments)
        {
            WebcamValue = arguments[Constants.Webcam.Parameters.WebcamValue];
        }
    }
}
