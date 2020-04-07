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

        public static Screen GetScreen(int width, int height, int x, int y)
        {
            var bounds = new Rectangle(x, y, width, height);
            return Screen.FromRectangle(bounds);
        }
    }
}
