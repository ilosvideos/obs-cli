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

namespace obs_cli.Commands.Implementations
{
    public class Initialize : BaseStartRecording
    {
        public string SavedAudioInputId { get; set; }
        public string SavedAudioOutputId { get; set; }

        public override string Name => AvailableCommand.Initialize.GetDescription();

        public Initialize(IDictionary<string, string> arguments)
            : base(arguments)
        {
            SavedAudioInputId = arguments["savedAudioInputId"];
            SavedAudioOutputId = arguments["savedAudioOutputId"];
        }

        public override void Execute()
        {
            if (!Obs.Startup("en-US"))
            {
                // todo: if any exceptions are thrown in this app, we need to bubble it all up to a single terminate code so consuming apps know that it shut down
                throw new ApplicationException("Startup failed.");
            }

            AudioService.ResetAudioInfo();

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

            var usedAudioInputId = AudioService.SetAudioInput(this.SavedAudioInputId);
            var usedAudioOutputId = AudioService.SetAudioOutput(this.SavedAudioOutputId);

            Store.Data.Obs.Presentation.SetItem(0);
            Store.Data.Obs.Presentation.SetSource(0);

            EmitService.EmitInitializeResponse(new InitializeResponse()
            {
                IsSuccessful = true,
                SetAudioInputDevice = usedAudioInputId,
                SetAudioOutputDevice = usedAudioOutputId
            });
        }
    }
}
