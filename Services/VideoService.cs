using OBS;
using obs_cli.Data;
using obs_cli.Helpers;
using obs_cli.Objects;
using System.IO;
using static OBS.libobs;

namespace obs_cli.Services
{
    public static class VideoService
    {
        public const int MAX_FPS = 30;
        public const int MIN_FPS = 4;

        /// <summary>
        /// Cancels the current recording.
        /// </summary>
        public static void CancelRecording()
        {
            if (Store.Data.Record.OutputAndEncoders != null)
            {
                Store.Data.Record.OutputAndEncoders.Dispose();
            }

            foreach (FileInfo file in Store.Data.Record.RecordedFiles)
            {
                file.Delete();
            }

            Store.Data.ResetRecordModule();
        }

        /// <summary>
        /// Configures the recording session for webcam only.
        /// </summary>
        /// <param name="frameRate"></param>
        /// <returns></returns>
        public static bool ConfigureWebcamOnly(uint frameRate)
        {
            Store.Data.Obs.WebcamScene.ClearItems();
            Store.Data.Obs.WebcamScene.Add(Store.Data.Webcam.Source);
            Store.Data.Obs.WebcamScene.Add(Store.Data.Audio.InputSource);
            Store.Data.Obs.Presentation.SetScene(Store.Data.Obs.WebcamScene);

            var obsVideoInfo = new GenerateObsVideoInfoParameters
            {
                BaseWidth = Store.Data.Webcam.Source.Width,
                OutputWidth = Store.Data.Webcam.Source.Width,
                BaseHeight = Store.Data.Webcam.Source.Height,
                OutputHeight = Store.Data.Webcam.Source.Height,
                FrameRate = GetFrameRate(frameRate)
            };

            obs_video_info ovi = ObsHelper.GenerateObsVideoInfoObject(obsVideoInfo);

            if (!Obs.ResetVideo(ovi))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Resets and updates the video settings for video output.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static bool ResetVideoInfo(ResetVideoInfoParameters parameters)
        {
            if (Store.Data.Obs.Presentation != null)
            {
                if (Store.Data.Obs.Presentation.SelectedScene.GetName().ToLowerInvariant() != "main")
                {
                    Store.Data.Obs.Presentation.SetScene(0);
                }
            }

            Store.Data.Record.AppliedCrop = new obs_sceneitem_crop
            {
                left = parameters.CropLeft,
                top = parameters.CropTop,
                right = parameters.CropRight,
                bottom = parameters.CropBottom
            };

            Store.Data.Record.ActiveScreen = ScreenHelper.GetScreen(parameters.ScreenX, parameters.ScreenY);

            //Set the proper display source
            if (Store.Data.Display.DisplaySource != null)
            {
                ObsData displaySettings = new ObsData();
                displaySettings.SetBool("capture_cursor", true);

                displaySettings.SetInt("monitor", ObsHelper.GetObsDisplayValueFromScreen(Store.Data.Display.DisplaySource, Store.Data.Record.ActiveScreen));

                Store.Data.Display.DisplaySource.Update(displaySettings);
                displaySettings.Dispose();
            }

            //Set the proper display bounds and crop
            if (Store.Data.Display.DisplayItem != null)
            {
                Store.Data.Display.DisplayItem.SetBounds(new Vector2(0, 0), ObsBoundsType.None, ObsAlignment.Top);
                Store.Data.Display.DisplayItem.SetCrop(Store.Data.Record.AppliedCrop);
            }

            WebcamService.CalculateItemPosition();

            var obsVideoInfo = new GenerateObsVideoInfoParameters
            {
                BaseWidth = (uint)parameters.CanvasWidth,
                OutputWidth = (uint)parameters.OutputWidth,
                BaseHeight = (uint)parameters.CanvasHeight,
                OutputHeight = (uint)parameters.OutputHeight,
                FrameRate = GetFrameRate(parameters.FrameRate)
            };

            obs_video_info ovi = ObsHelper.GenerateObsVideoInfoObject(obsVideoInfo);

            if (!Obs.ResetVideo(ovi))
            {
                return false;
            }

            Store.Data.Record.CanvasHeight = parameters.CanvasHeight;
            Store.Data.Record.CanvasWidth = parameters.CanvasWidth;

            return true;
        }

        /// <summary>
        /// Gets the appropriate frame rate.
        /// </summary>
        /// <param name="frameRate"></param>
        /// <returns></returns>
        private static uint GetFrameRate(uint frameRate)
        {
            int fps = (int)frameRate;

            if (fps < 4)
            {
                fps = MIN_FPS;
            }
            else if (fps > MAX_FPS)
            {
                fps = MAX_FPS;
            }

            return (uint)fps;
        }
    }
}
