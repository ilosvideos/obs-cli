using obs_cli.Data;
using obs_cli.Helpers;
using obs_cli.Objects;
using obs_cli.Objects.Obs;
using obs_cli.Services;
using System;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class StartRecording : ICommand
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
        public IntPtr ScreenToRecordHandle { get; set; }
        public string VideoOutputFolder { get; set; }

        public static string Name
        {
            get
            {
                return "start-recording";
            }
        }

        public StartRecording(IDictionary<string, string> arguments)
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
            this.ScreenToRecordHandle = (IntPtr)int.Parse(arguments["screenToRecordHandle"]);
            this.VideoOutputFolder = arguments["videoOutputFolder"];
        }

        public void Execute()
        {
            FileWriteService.WriteLineToFile("in start recording execute");

            try 
            {
                bool resetVideoInfoStatus = VideoService.ResetVideoInfo(new ResetVideoInfoParameters
                {
                    CropTop = CropTop,
                    CropRight = CropRight,
                    CropLeft = CropLeft,
                    CropBottom = CropBottom,
                    FrameRate = FrameRate,
                    OutputWidth = OutputWidth,
                    OutputHeight = OutputHeight,
                    CanvasWidth = CanvasWidth,
                    CanvasHeight = CanvasHeight,
                    ScreenToRecordHandle = ScreenToRecordHandle
                });

                FileWriteService.WriteLineToFile($"ResetVideoInfo status: {resetVideoInfoStatus}");

                ObsOutputAndEncoders outputAndEncoders = ObsService.CreateNewObsOutput(VideoOutputFolder);
                Store.Data.Record.OutputAndEncoders = outputAndEncoders;
                Store.Data.Record.OutputAndEncoders.obsOutput.Start();

                FileWriteService.WriteLineToFile("recording started");
            }
            catch(Exception ex)
            {
                FileWriteService.WriteLineToFile(ex.Message);
            }
        }
    }
}
