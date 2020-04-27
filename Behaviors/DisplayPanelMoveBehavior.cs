using obs_cli.Controls;
using obs_cli.Utility;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Application = System.Windows.Application;

namespace obs_cli.Behaviors
{
    internal static class DisplayPanelMoveBehavior
    {
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public static void Attach(DisplayPanel child, Window window)
        {
            if (child.Name.Equals("")) child.Name = Guid.NewGuid().ToString();
            Application.Current.Properties["window_" + child.Name] = window;
            Application.Current.Properties["windowHandle_" + child.Name] = new WindowInteropHelper(window).Handle;

            child.MouseMove += Child_MouseMove;
            child.MouseDown += child_MouseDown;
            child.Disposed += Child_Disposed;
        }

        private static void Child_Disposed(object sender, EventArgs e)
        {
            ((DisplayPanel)sender).MouseMove -= Child_MouseMove;
            ((DisplayPanel)sender).MouseDown -= child_MouseDown;
        }

        private static void Child_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            DisplayPanel displayPanel = (DisplayPanel)sender;
            if (e.Button == MouseButtons.Left && (e.X != (int)Application.Current.Properties[displayPanel.Name + "_mouse_x"] || e.Y != (int)Application.Current.Properties[displayPanel.Name + "_mouse_y"]))
            {
                Window window = (Window)Application.Current.Properties["window_" + displayPanel.Name];
                IntPtr windowHandle = (IntPtr)Application.Current.Properties["windowHandle_" + displayPanel.Name];

                if (e.Button == MouseButtons.Left)
                {
                    window.ResizeMode = ResizeMode.NoResize;
                    ReleaseCapture();
                    SendMessage(windowHandle, (int)Constants.WindowMessage.WM_NCLBUTTONDOWN, (int)Constants.WindowMessage.HTCAPTION, 0);
                }
            }
        }

        private static void child_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            DisplayPanel displayPanel = (DisplayPanel)sender;
            Application.Current.Properties[displayPanel.Name + "_mouse_x"] = e.X;
            Application.Current.Properties[displayPanel.Name + "_mouse_y"] = e.Y;
        }

    }
}
