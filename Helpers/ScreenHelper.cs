using System;
using System.Windows.Forms;

namespace obs_cli.Helpers
{
    public static class ScreenHelper
    {
        /// <summary>
        /// Gets the Screen that the object that the passed handle references.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static Screen GetScreen(IntPtr handle)
        {
            return Screen.FromHandle(handle);
        }
    }
}
