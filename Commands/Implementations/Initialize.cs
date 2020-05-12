using OBS;
using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Helpers;
using obs_cli.Objects;
using obs_cli.Objects.Obs;
using obs_cli.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace obs_cli.Commands.Implementations
{
    public class Initialize : BaseStartRecording
    {
        public string SavedAudioInputId { get; set; }
        public string SavedAudioOutputId { get; set; }
        public uint DpiX { get; set; }
        public uint DpiY { get; set; }

        public override string Name => AvailableCommand.Initialize.GetDescription();

        public Initialize(IDictionary<string, string> arguments)
            : base(arguments)
        {
            SavedAudioInputId = arguments["savedAudioInputId"];
            SavedAudioOutputId = arguments["savedAudioOutputId"];
            DpiX = uint.Parse(arguments["dpiX"]);
            DpiY = uint.Parse(arguments["dpiY"]);
        }

        public override void Execute()
        {
            FileWriteService.WriteLineToFile("initialize command start");

            if (!Obs.Startup("en-US"))
            {
                // todo: if any exceptions are thrown in this app, we need to bubble it all up to a single terminate code so consuming apps know that it shut down
                throw new ApplicationException("Startup failed.");
            }

            Store.Data.Display.SetSystemDpi(DpiX, DpiY);

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
            Store.Data.Obs.Presentation.AddScene("Main");
            Store.Data.Obs.Presentation.AddScene("Webcam");
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
