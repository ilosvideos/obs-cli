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
            try 
            {
                Store.Data.Record.VideoOutputFolder = VideoOutputFolder;

                // todo: this is pretty ugly. maybe make a separate "StartWebcamOnlyRecording" command? I like that a little more
                if (Store.Data.Webcam.IsWebcamOnly)
                {
                    ObsVideoService.ConfigureWebcamOnly(FrameRate);
                }
                else
                {
                    ObsVideoService.ResetVideoInfo(new ResetVideoInfoParameters
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
                }

                ObsOutputAndEncoders outputAndEncoders = ObsService.CreateNewObsOutput();
                Store.Data.Record.OutputAndEncoders = outputAndEncoders;
                Store.Data.Record.OutputAndEncoders.obsOutput.Start();

                EmitService.EmitStatusResponse(AvailableCommand.StartRecording, true);
            }
            catch(Exception ex)
            {
                FileWriteService.WriteLineToFile(ex.Message);
            }
        }
    }
}
