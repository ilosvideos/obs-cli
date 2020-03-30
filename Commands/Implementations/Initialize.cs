using OBS;
using obs_cli.Helpers;
using obs_cli.Utility;
using System;
using System.Collections.Generic;

namespace obs_cli.Commands.Implementations
{
    public class Initialize : ICommand
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public static string Name
        {
            get
            {
                return "initialize";
            }
        }

        public Initialize(IDictionary<string, string> arguments)
        {
            FileWriteService.WriteToFile($"got {arguments.Count} arguments");

            this.X = int.Parse(arguments["x"]);
            this.Y = int.Parse(arguments["y"]);
            this.Width = int.Parse(arguments["width"]);
            this.Height = int.Parse(arguments["height"]);

            FileWriteService.WriteToFile($"Initializing with X position: {X}");
            FileWriteService.WriteToFile($"Initializing with Y position: {Y}");
            FileWriteService.WriteToFile($"Initializing with Width: {Width}");
            FileWriteService.WriteToFile($"Initializing with Height position: {Height}");
        }

        public void Execute()
        {
            FileWriteService.WriteToFile("initialize command start");

            if (!Obs.Startup("en-US"))
            {
                // todo: if any exceptions are thrown in this app, we need to bubble it all up to a single terminate code so consuming apps know that it shut down
                throw new ApplicationException("Startup failed.");
            }

            ResetAudioInfo();

            FileWriteService.WriteToFile("ResetAudioInfo successful");

            // todo: ResetVideoInfo();

            Obs.LoadAllModules();

            FileWriteService.WriteToFile("Obs.LoadAllModules successful");

            FileWriteService.WriteToFile("initialize command end");
        }

        private void ResetAudioInfo()
        {
            libobs.obs_audio_info avi = new libobs.obs_audio_info
            {
                samples_per_sec = Constants.Audio.SAMPLES_PER_SEC,
                speakers = libobs.speaker_layout.SPEAKERS_STEREO
            };

            if (!Obs.ResetAudio(avi))
                throw new ApplicationException("ResetAudio failed.");
        }

        //private void ResetVideoInfo()
        //{
        //    if (presentation != null)
        //    {
        //        if (presentation.SelectedScene.GetName().ToLowerInvariant() != "main")
        //        {
        //            presentation.SetScene(0);
        //        }
        //    }

        //    // When recording in fullscreen, never switch screens. At least until we have a good way to scale videos with different resolutions when concatenating
        //    Screen screenToRecord = screenForFullscreenRecording ?? Util.GetActiveScreen();
        //    Rect activeScreenBounds = DpiUtil.GetScreenWpfBounds(screenToRecord);
        //    _physicalRecordingScreen = screenToRecord;

        //    // Fullscreen
        //    if (MainWindowAccessor.Window.selectionWindow == null)
        //    {
        //        System.Windows.Point obsAdj = DpiUtil.GetWpfSizeAdjustmentForObs(activeScreenBounds, activeScreenBounds.Width, activeScreenBounds.Height, true);
        //        Rect adjustedActiveScreenBounds = activeScreenBounds;
        //        adjustedActiveScreenBounds.Width += obsAdj.X;
        //        adjustedActiveScreenBounds.Height += obsAdj.Y;

        //        int originalCanvasWidth = DpiUtil.ConvertSizeWpfToPhysicalPixel(activeScreenBounds.Width);
        //        int originalCanvasHeight = DpiUtil.ConvertSizeWpfToPhysicalPixel(activeScreenBounds.Height);
        //        canvasWidth = DpiUtil.ConvertSizeWpfToPhysicalPixel(adjustedActiveScreenBounds.Width);
        //        canvasHeight = DpiUtil.ConvertSizeWpfToPhysicalPixel(adjustedActiveScreenBounds.Height);

        //        int recordingWidthAdj = originalCanvasWidth - canvasWidth;
        //        int recordingHeightAdj = originalCanvasHeight - canvasHeight;
        //        int cropLeft = recordingWidthAdj / 2;
        //        int cropTop = recordingHeightAdj / 2;
        //        int cropRight = recordingWidthAdj - cropLeft;
        //        int cropBottom = recordingHeightAdj - cropTop;

        //        AppliedCrop = new libobs.obs_sceneitem_crop
        //        {
        //            left = cropLeft,
        //            top = cropTop,
        //            right = cropRight,
        //            bottom = cropBottom
        //        };
        //    }
        //    // Selection window
        //    else
        //    {
        //        // We need to get these values from the main thread and save them got references
        //        Util.WindowSizeProperties selectionWindowSize = GetWindowSizeFromMainThread(MainWindowAccessor.Window.selectionWindow);
        //        canvasWidth = DpiUtil.ConvertSizeWpfToPhysicalPixel(selectionWindowSize.Width - (selectionWindowSize.BorderWidth.Value * 2));
        //        canvasHeight = DpiUtil.ConvertSizeWpfToPhysicalPixel(selectionWindowSize.Height - (selectionWindowSize.BorderWidth.Value * 2));

        //        int cropLeft = DpiUtil.ConvertSizeWpfToPhysicalPixel(selectionWindowSize.Left - activeScreenBounds.X + selectionWindowSize.BorderWidth.Value);
        //        int cropTop = DpiUtil.ConvertSizeWpfToPhysicalPixel(selectionWindowSize.Top - activeScreenBounds.Y + selectionWindowSize.BorderWidth.Value);
        //        int cropRight = DpiUtil.ConvertSizeWpfToPhysicalPixel(activeScreenBounds.Width) - cropLeft - canvasWidth;
        //        int cropBottom = DpiUtil.ConvertSizeWpfToPhysicalPixel(activeScreenBounds.Height) - cropTop - canvasHeight;

        //        AppliedCrop = new libobs.obs_sceneitem_crop
        //        {
        //            left = cropLeft,
        //            top = cropTop,
        //            right = cropRight,
        //            bottom = cropBottom,
        //        };
        //    }

        //    // Set the proper display source
        //    if (source_display != null)
        //    {
        //        ObsData displaySettings = new ObsData();
        //        displaySettings.SetBool("capture_cursor", true);
        //        displaySettings.SetInt("monitor", GetObsDisplayValueFromScreen(screenToRecord));
        //        source_display.Update(displaySettings);
        //        displaySettings.Dispose();
        //    }

        //    // Set the proper display bounds and crop
        //    if (item_display != null)
        //    {
        //        item_display.SetBounds(new Vector2(0, 0), ObsBoundsType.None, ObsAlignment.Top);
        //        item_display.SetCrop(AppliedCrop);
        //    }

        //    CalculateWebcamItemPosition();

        //    libobs.obs_video_info ovi = GenerateObsVideoInfoObject(
        //        (uint)canvasWidth,
        //        (uint)canvasHeight,
        //        (uint)DpiUtil.ConvertSizeRectDpiToSystemDpi(activeScreenBounds, canvasWidth),
        //        (uint)DpiUtil.ConvertSizeRectDpiToSystemDpi(activeScreenBounds, canvasHeight));

        //    if (!Obs.ResetVideo(ovi))
        //        throw new ApplicationException("ResetVideo failed.");
        //}
    }
}
