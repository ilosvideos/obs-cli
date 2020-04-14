using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Helpers;
using obs_cli.Objects;
using obs_cli.Objects.Obs;
using obs_cli.Services;
using System;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class StartRecording : BaseStartRecording
    {
        public string VideoOutputFolder { get; set; }

        public override string Name => AvailableCommand.StartRecording.GetDescription();

        public StartRecording(IDictionary<string, string> arguments)
            : base(arguments)
        {
            this.VideoOutputFolder = arguments["videoOutputFolder"];
        }

        public override void Execute()
        {
            FileWriteService.WriteLineToFile("in start recording execute");

            try 
            {
                Store.Data.Record.VideoOutputFolder = VideoOutputFolder;

                bool resetVideoInfoStatus = ObsVideoService.ResetVideoInfo(new ResetVideoInfoParameters
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

                ObsOutputAndEncoders outputAndEncoders = ObsService.CreateNewObsOutput();
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
