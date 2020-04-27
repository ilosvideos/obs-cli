using obs_cli.Objects.Obs;
using obs_cli.Utility;
using obs_cli.Windows;
using System.Collections.Generic;
using WebcamDevice = obs_cli.Objects.Webcam;

namespace obs_cli.Data.Modules
{
    public class Webcam
    {
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
    }
}
