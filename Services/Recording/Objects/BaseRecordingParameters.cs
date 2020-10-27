using System;

namespace obs_cli.Services.Recording.Objects
{
    public class BaseRecordingParameters
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
        public string VideoOutputFolder { get; set; }
        public int ScreenX { get; set; }
        public int ScreenY { get; set; }
    }
}
