using OBS;
using obs_cli.Objects.Obs;
using obs_cli.Structs;
using obs_cli.Utility;
using obs_cli.Windows;
using System;
using System.Collections.Generic;
using System.Windows;
using WebcamDevice = obs_cli.Objects.Webcam;

namespace obs_cli.Data.Modules
{
    public class Webcam
    {
        public string ActiveWebcamValue { get; set; }

        public Item Item { get; set; }

        public Source Source { get; set; }

        public WebcamWindow Window { get; set; }

        public List<WebcamDevice> Webcams { get; set; }

        public Webcam()
        {
            Webcams = new List<WebcamDevice>();
        }

        public List<WebcamDevice> GetWebcams()
        {
            if (Window == null)
            {
                Window = new WebcamWindow();
            }

            return Webcams;
        }

        /// <summary>
        /// Disposes the OBS webcam item and source and reenables standalone audio input.
        /// </summary>
        public void DestroyObsWebcam()
        {
            Store.Data.Obs.MainScene.Items.Remove(Item);
            Item.Remove();
            Item.Dispose();
            Item = null;

            Store.Data.Obs.Presentation.Sources.Remove(Source);
            Source.Enabled = false;
            Source.Remove();
            Source.Dispose();
            Source = null;

            Store.Data.Audio.InputSource.Enabled = true;
            Store.Data.Audio.InputSource.AudioOffset = Constants.Audio.DELAY_INPUT;
        }

        /// <summary>
        /// Moves the OBS webcam item off screen.
        /// </summary>
        public void SetWebcamItemOffscreen()
        {
            Item.Position = new Vector2(Store.Data.Record.CanvasWidth, Store.Data.Record.CanvasHeight);
        }

        /// <summary>
        /// Calculates where the OBS webcam item should be positioned.
        /// </summary>
        public void CalculateItemPosition()
        {
            if (Item == null)
            {
                return;
            }            

            // todo: verify that this is the correct screen
            Rect activeScreenBounds = DpiUtil.GetScreenWpfBounds(Store.Data.Record.ActiveScreen);

            double baseOffsetX = activeScreenBounds.X;
            double baseOffsetY = activeScreenBounds.Y;

            // todo: determine if we're fullscreen or not. pass a value over to the cli when fullscreen is toggled on/off
            //if (MainWindowAccessor.Window.selectionWindow != null)
            //{
            //    ILog.IlosLogger.Trace("Calculating webcam non-fullscreen");
            //    Util.WindowSizeProperties selectionWindowSize = Util.GetWindowSizeFromMainThread(MainWindowAccessor.Window.selectionWindow);
            //    baseOffsetX = selectionWindowSize.Left + selectionWindowSize.BorderWidth.Value;
            //    baseOffsetY = selectionWindowSize.Top + selectionWindowSize.BorderWidth.Value;
            //}

            WindowSizeProperties webcamWindowSize = Util.GetWindowSizeProperties(Store.Data.Webcam.Window);
            double webcamItemWidth = webcamWindowSize.Width - (webcamWindowSize.BorderWidth.Value * 2);
            double webcamItemHeight = webcamWindowSize.Height - (webcamWindowSize.BorderWidth.Value * 2);

            int webcamObsX = DpiUtil.ConvertSizeWpfToPhysicalPixel(webcamWindowSize.Left + webcamWindowSize.BorderWidth.Value - baseOffsetX);
            int webcamObsY = DpiUtil.ConvertSizeWpfToPhysicalPixel(webcamWindowSize.Top + webcamWindowSize.BorderWidth.Value - baseOffsetY);
            int webcamObsWidth = DpiUtil.ConvertSizeWpfToPhysicalPixel(webcamItemWidth);
            int webcamObsHeight = DpiUtil.ConvertSizeWpfToPhysicalPixel(webcamItemHeight);

            Store.Data.Webcam.Item.Position = new Vector2((float)webcamObsX, (float)webcamObsY);
            Store.Data.Webcam.Item.SetBounds(new Vector2(webcamObsWidth, webcamObsHeight), ObsBoundsType.ScaleOuter, ObsAlignment.Center);

            double webcamAspectRatio = Convert.ToDouble(Store.Data.Webcam.Source.Width) / Convert.ToDouble(Store.Data.Webcam.Source.Height);
            double webcamItemAspectRatio = Convert.ToDouble(webcamItemWidth) / Convert.ToDouble(webcamItemHeight);
            double webcamWidthD = Convert.ToDouble(Store.Data.Webcam.Source.Width);
            double webcamHeightD = Convert.ToDouble(Store.Data.Webcam.Source.Height);
            int cropWidth = webcamItemAspectRatio < webcamAspectRatio ? Convert.ToInt32((webcamWidthD - (webcamHeightD * webcamItemAspectRatio)) / 2) : 0;
            int cropHeight = webcamItemAspectRatio < webcamAspectRatio ? 0 : Convert.ToInt32((webcamHeightD - (webcamWidthD / webcamItemAspectRatio)) / 2);
            libobs.obs_sceneitem_crop webcamCrop = new libobs.obs_sceneitem_crop
            {
                left = webcamItemAspectRatio < webcamAspectRatio ? cropWidth : 0,
                top = webcamItemAspectRatio < webcamAspectRatio ? 0 : cropHeight,
                right = webcamItemAspectRatio < webcamAspectRatio ? cropWidth : 0,
                bottom = webcamItemAspectRatio < webcamAspectRatio ? 0 : cropHeight
            };

            Store.Data.Webcam.Item.SetCrop(webcamCrop);
        }
    }
}
