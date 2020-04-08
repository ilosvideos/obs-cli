using System;
using System.Drawing;
using System.Windows.Forms;

namespace obs_cli.Helpers
{
    public static class ScreenHelper
    {
        public static Screen GetScreen(IntPtr handle)
        {
            return Screen.FromHandle(handle);
        }
    }
}
