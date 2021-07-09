using obs_cli.Objects;
using System.Collections.Generic;

namespace obs_cli.Commands
{
    public abstract class BaseStartRecording : BaseCommand
    {
        public int CropTop { get; set; }
        public int CropRight { get; set; }
        public int CropLeft { get; set; }
        public int CropBottom { get; set; }
        public uint FrameRate { get; set; }
        public double OutputWidth { get; set; }
        public double OutputHeight { get; set; }
        public int CanvasWidth { get; set; }
        public int CanvasHeight { get; set; }
        public int ScreenX { get; set; }
        public int ScreenY { get; set; }

        protected BaseStartRecording(IDictionary<string, string> arguments)
        {
            this.CropTop = int.Parse(arguments["cropTop"]);
            this.CropRight = int.Parse(arguments["cropRight"]);
            this.CropBottom = int.Parse(arguments["cropBottom"]);
            this.CropLeft = int.Parse(arguments["cropLeft"]);
            this.FrameRate = uint.Parse(arguments["frameRate"]);
            this.CanvasWidth = int.Parse(arguments["canvasWidth"]);
            this.CanvasHeight = int.Parse(arguments["canvasHeight"]);
            this.OutputWidth = double.Parse(arguments["outputWidth"]);
            this.OutputHeight = double.Parse(arguments["outputHeight"]);
            this.ScreenX = int.Parse(arguments["screenX"]);
            this.ScreenY = int.Parse(arguments["screenY"]);

            Loggers.CliLogger.Info($"Received CanvasWidth: {this.CanvasWidth}");
            Loggers.CliLogger.Info($"Received CanvasHeight: {this.CanvasHeight}");
            Loggers.CliLogger.Info($"Received OutputWidth: {this.OutputWidth}");
            Loggers.CliLogger.Info($"Received OutputHeight: {this.OutputHeight}");
            Loggers.CliLogger.Info($"Received ScreenX: {this.ScreenX}");
            Loggers.CliLogger.Info($"Received ScreenY: {this.ScreenY}");
        }
    }
}
