using obs_cli.Data;
using obs_cli.Enums;
using System.Collections.Generic;
using System.Linq;

namespace obs_cli.Commands.Implementations
{
    public class ChangeWebcam : BaseCommand
    {
        public string WebcamValue { get; set; }

        public override string Name => AvailableCommand.ChangeWebcam.GetDescription();

        // todo: move this to base class and have EnableWebcam extend from it too
        public ChangeWebcam(IDictionary<string, string> arguments)
        {
            WebcamValue = arguments["webcamValue"];
        }

        public override void Execute()
        {
            if (WebcamValue == Store.Data.Webcam.ActiveWebcamValue)
            {
                return;
            }

            var webcam = Store.Data.Webcam.Webcams.FirstOrDefault(x => x.value == WebcamValue);
            if (webcam == null)
            {
                return;
            }

            Store.Data.Webcam.Window.setWebcam(webcam);
        }
    }
}
