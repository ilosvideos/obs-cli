using System.Collections.Generic;

namespace obs_cli.Commands.Abstract
{
    public abstract class BaseWebcamInitialization : BaseCommand
    {
        public string WebcamValue { get; set; }

        protected BaseWebcamInitialization(IDictionary<string, string> arguments)
        {
            WebcamValue = arguments["webcamValue"];
        }
    }
}
