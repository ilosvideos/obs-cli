using obs_cli.Structs;
using obs_cli.Windows;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace obs_cli.Utility
{
    public static class Util
    {
        public static double NearestMultiple(double value, double factor)
        {
            return Math.Round(
                         (value / factor),
                         MidpointRounding.AwayFromZero
                     ) * factor;
        }

        // OBS requires OUTPUT width divisible by 4 and height divisible by 2 - give it the recording width and height
        // We require height to be divisible by 4 because that makes it easy for scaling
        // Returning Point because Size can't have negative values
        public static System.Windows.Point GetObsResolutionAdjustment(double width, double height, bool shrinkOnly)
        {
            double recordingWidthNearest = NearestMultiple(width, 4d);
            double recordingHeightNearest = NearestMultiple(height, 4d);

            double recordingWidthAdj = recordingWidthNearest - width;
            double recordingHeightAdj = recordingHeightNearest - height;

            if (shrinkOnly)
            {
                if (recordingWidthAdj > 0) recordingWidthAdj = recordingWidthAdj - 4d;
                if (recordingHeightAdj > 0) recordingHeightAdj = recordingHeightAdj - 4d;
            }

            return new System.Windows.Point(recordingWidthAdj, recordingHeightAdj);
        }

        /// <summary>
        /// Gets the window's size properties
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public static WindowSizeProperties GetWindowSizeProperties(Window window)
        {
            WindowSizeProperties properties = new WindowSizeProperties();

            window.Dispatcher.Invoke(new Action(() =>
            {
                properties.Width = window.Width;
                properties.Height = window.Height;
                properties.Left = window.Left;
                properties.Top = window.Top;
            }));

            // todo: I don't think this is required. we might need to do this logic differently if we need this
            //if (window.GetType() == typeof(SelectionWindow))
            //{
            //    SelectionWindow selectionWindow = (SelectionWindow)window;
            //    properties.BorderWidth = selectionWindow.BorderSize;
            //}

            if (window.GetType() == typeof(WebcamWindow))
            {
                WebcamWindow webcamWindow = (WebcamWindow)window;
                properties.BorderWidth = webcamWindow.BorderSize;
            }

            return properties;
        }
    }
}
