using obs_cli.Utility;
using System;
using System.Windows;
using System.Windows.Controls;
using Control = System.Windows.Forms.Control;

namespace obs_cli.Controls
{
    /// <summary>
    /// Interaction logic for WindowsFormsHostOverlay.xaml
    /// </summary>
    public partial class WindowsFormsHostOverlay : Window
    {
        Border t;

        public WindowsFormsHostOverlay(Border target, Control child)
        {
            InitializeComponent();

            t = target;
            wfh.Child = child;

            t.Dispatcher.Invoke(new Action(() =>
            {
                Owner = GetWindow(t);
                Owner.LocationChanged += new EventHandler(EventHandler);
                t.SizeChanged += new SizeChangedEventHandler(EventHandler);
                PositionAndResize();

                if (Owner.IsVisible)
                {
                    Show();
                }
                else
                {
                    Owner.IsVisibleChanged += delegate
                    {
                        if (Owner.IsVisible && IsLoaded)
                        {
                            Show();
                        }
                    };
                }
            }));
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Owner.LocationChanged -= new EventHandler(EventHandler);
            t.SizeChanged -= new SizeChangedEventHandler(EventHandler);
        }

        private void EventHandler(object sender, EventArgs e)
        {
            PositionAndResize();
        }

        public void PositionAndResize()
        {
            double padding = DpiUtil.ConvertSizeSystemDpiToMonitorDpi(Owner, ((Border)(t.Parent)).Padding.Left);

            Left = Owner.Left + padding;
            Top = Owner.Top + padding;

            // Don't allow negatives
            Width = Math.Max((Owner.Width - 2 * padding), 0);
            Height = Math.Max((Owner.Height - 2 * padding), 0);
        }
    };

}