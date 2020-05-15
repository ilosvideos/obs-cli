using MonitorAware.Models;
using obs_cli.Objects.Obs;
using System;
using System.Reflection;
using System.Windows;

namespace obs_cli.Data.Modules
{
    public class Display
    {
        public Source DisplaySource { get; set; }
        public Item DisplayItem { get; set; }
        public Dpi SystemDpi { 
            get
            {
                var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
                var dpiYProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);

                var dpiX = (int)dpiXProperty.GetValue(null, null);
                var dpiY = (int)dpiYProperty.GetValue(null, null);

                return new Dpi(Convert.ToUInt32(dpiX), Convert.ToUInt32(dpiY));
            } 
        }
    }
}
