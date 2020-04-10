using OBS;
using obs_cli.Data;
using obs_cli.Helpers;
using obs_cli.Objects;
using System.Windows.Forms;
using static OBS.libobs;

namespace obs_cli.Services
{
    public static class VideoService
    {
        public const int MAX_FPS = 30;
        public const int MIN_FPS = 4;

        /// <summary>
        /// Gets the appropriate frame rate.
        /// </summary>
        /// <param name="frameRate"></param>
        /// <returns></returns>
        public static uint GetFrameRate(uint frameRate)
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

            //Set the proper display source
            if (Store.Data.Display.DisplaySource != null)
            {
                ObsData displaySettings = new ObsData();
                displaySettings.SetBool("capture_cursor", true);

                Screen activeScreen = ScreenHelper.GetScreen(parameters.ScreenToRecordHandle);
                displaySettings.SetInt("monitor", ObsHelper.GetObsDisplayValueFromScreen(Store.Data.Display.DisplaySource, activeScreen));

                Store.Data.Display.DisplaySource.Update(displaySettings);
                displaySettings.Dispose();
            }

            //Set the proper display bounds and crop
            if (Store.Data.Display.DisplayItem != null)
            {
                Store.Data.Display.DisplayItem.SetBounds(new Vector2(0, 0), ObsBoundsType.None, ObsAlignment.Top);
                Store.Data.Display.DisplayItem.SetCrop(Store.Data.Record.AppliedCrop);
            }

            // todo: webcam related
            //CalculateWebcamItemPosition();

            obs_video_info ovi = ObsHelper.GenerateObsVideoInfoObject(
                (uint)parameters.CanvasWidth,
                (uint)parameters.CanvasHeight,
                (uint)parameters.OutputWidth,
                (uint)parameters.OutputHeight,
                GetFrameRate(parameters.FrameRate));

            if (!Obs.ResetVideo(ovi))
            {
                return false;
            }

            return true;
        }
    }
}
