using OBS;
using obs_cli.Data;
using obs_cli.Helpers;
using obs_cli.Objects;
using obs_cli.Objects.Obs;
using obs_cli.Services;
using obs_cli.Structs;
using obs_cli.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static OBS.libobs;

namespace obs_cli.Commands.Implementations
{
    public class Initialize : ICommand
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
        public string SavedAudioInputId { get; set; }
        public string SavedAudioOutputId { get; set; }

        public static string Name
        {
            get
            {
                return "initialize";
            }
        }

        public Initialize(IDictionary<string, string> arguments)
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
            this.SavedAudioInputId = arguments["savedAudioInputId"];
            this.SavedAudioOutputId = arguments["savedAudioOutputId"];
        }

        public void Execute()
        {
            FileWriteService.WriteLineToFile("initialize command start");

            if (!Obs.Startup("en-US"))
            {
                // todo: if any exceptions are thrown in this app, we need to bubble it all up to a single terminate code so consuming apps know that it shut down
                throw new ApplicationException("Startup failed.");
            }

            AudioService.ResetAudioInfo();

            FileWriteService.WriteLineToFile("ResetAudioInfo successful");

            VideoService.ResetVideoInfo(new ResetVideoInfoParameters
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

            Obs.LoadAllModules();

            FileWriteService.WriteLineToFile("Obs.LoadAllModules successful");

            Store.Data.Obs.Presentation = new Presentation();
            Store.Data.Obs.MainScene = Store.Data.Obs.Presentation.AddScene("Main");
            Store.Data.Obs.WebcamScene = Store.Data.Obs.Presentation.AddScene("Webcam");
            Store.Data.Obs.Presentation.SetScene(Store.Data.Obs.MainScene);

            Store.Data.Display.DisplaySource = Store.Data.Obs.Presentation.CreateSource("monitor_capture", "Monitor Capture Source");

            Store.Data.Obs.Presentation.AddSource(Store.Data.Display.DisplaySource);
            Store.Data.Display.DisplayItem = Store.Data.Obs.Presentation.CreateItem(Store.Data.Display.DisplaySource);
            Store.Data.Display.DisplayItem.Name = "Monitor Capture SceneItem";

            Rectangle activeScreenBounds = ScreenHelper.GetScreen(this.ScreenToRecordHandle).Bounds;

            Store.Data.Display.DisplayItem.SetBounds(new Vector2(activeScreenBounds.Width, activeScreenBounds.Height), ObsBoundsType.None, ObsAlignment.Top); // this should always be the screen's resolution
            Store.Data.Obs.MainScene.Items.Add(Store.Data.Display.DisplayItem);

            AudioService.SetAudioInput(this.SavedAudioInputId);
            AudioService.SetAudioOutput(this.SavedAudioOutputId);

            Store.Data.Obs.Presentation.SetItem(0);
            Store.Data.Obs.Presentation.SetSource(0);

            FileWriteService.WriteLineToFile("initialize command end");
        }
    }
}
