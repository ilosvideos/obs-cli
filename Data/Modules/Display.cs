using MonitorAware.Models;
using obs_cli.Objects.Obs;

namespace obs_cli.Data.Modules
{
    public class Display
    {
        public Source DisplaySource { get; set; }
        public Item DisplayItem { get; set; }
        public uint DpiX { get; set; }
        public uint DpiY { get; set; }
        public Dpi SystemDpi { 
            get
            {
                return new Dpi(DpiX, DpiY);
            } 
        }

        public void SetSystemDpi(uint dpiX, uint dpiY)
        {
            DpiX = dpiX;
            DpiY = dpiY;
        }
    }
}
