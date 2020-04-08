using System;

namespace obs_cli.Objects
{
    public class ResetVideoInfoParameters
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
        public double ActiveScreenBoundsWidth { get; set; }
        public double ActiveScreenBoundsHeight { get; set; }
        public double ActiveScreenBoundsX { get; set; }
        public double ActiveScreenBoundsY { get; set; }
    }
}
