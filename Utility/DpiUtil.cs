using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using MonitorAware.Models;
using MonitorAware.Views;

namespace obs_cli.Utility
{
    public static class DpiUtil
    {
        public static Dpi GetSystemDpi()
        {
            // todo: pass the system DPI in the initialize. store in Obs data module
            return new Dpi(0, 0);
            //return ((MainWindow)MainWindowAccessor.Window).MonitorProperty.WindowHandler.SystemDpi;
        }

        public static Rect GetScreenWpfBounds(Screen screen)
        {
            Dpi systemDpi = GetSystemDpi();

            Rect wpfBounds = new Rect(screen.Bounds.X, screen.Bounds.Y, screen.Bounds.Width, screen.Bounds.Height);

            wpfBounds.X = wpfBounds.X * (double)Dpi.Default.X / (double)systemDpi.X;
            wpfBounds.Y = wpfBounds.Y * (double)Dpi.Default.Y / (double)systemDpi.Y;
            wpfBounds.Width = wpfBounds.Width * (double)Dpi.Default.X / (double)systemDpi.X;
            wpfBounds.Height = wpfBounds.Height * (double)Dpi.Default.Y / (double)systemDpi.Y;

            return wpfBounds;
        }

        public static double ConvertSizeSystemDpiToRectDpi(Rect rect, double size)
        {
            Dpi systemDpi = GetSystemDpi();
            Dpi rectDpi = DpiChecker.GetDpiFromRect(rect);

            return size * rectDpi.X / systemDpi.X;
        }

        public static double ConvertSizeSystemDpiToMonitorDpi(Window window, double size)
        {
            Dpi systemDpi = GetSystemDpi();
            Dpi screenDpi = MonitorAwareProperty.GetAttachedProperty(window).WindowHandler.MonitorDpi;

            return size * screenDpi.X / systemDpi.X;
        }

        public static double ConvertSizeRectDpiToSystemDpi(Rect rect, double size)
        {
            Dpi systemDpi = GetSystemDpi();
            Dpi rectDpi = DpiChecker.GetDpiFromRect(rect);

            return size * systemDpi.X / rectDpi.X;
        }

        public static double ConvertSizeMonitorDpiToSystemDpi(Window window, double size)
        {
            Dpi systemDpi = GetSystemDpi();
            Dpi screenDpi = MonitorAwareProperty.GetAttachedProperty(window).WindowHandler.MonitorDpi;

            return size * systemDpi.X / screenDpi.X;
        }

        public static int ConvertSizeWpfToPhysicalPixel(double size)
        {
            Dpi systemDpi = GetSystemDpi();

            return (int)Math.Round(size * (double)systemDpi.X / (double)MonitorAware.Models.Dpi.Default.X);
        }

        public static double ConvertSizePhysicalPixelToWpf(double size)
        {
            Dpi systemDpi = GetSystemDpi();

            return size * (double)MonitorAware.Models.Dpi.Default.X / (double)systemDpi.X;
        }

        /// <summary>
        /// Gets a ScaleTransform used to manually scale elements to be the correct size on monitors with non-system DPI. The Rect passed in is used to find the monitor DPI that the rect is on. The Rect should be the WPF System-DPI coordinates. 
        /// </summary>
        /// <param name="rect">WPF System-DPI coordinates</param>
        /// <returns>ScaleTransform with a scale factor that is Monitor DPI / System DPI</returns>
        public static ScaleTransform GetScaleTransformForRect(Rect rect)
        {
            double monitorScaleFactorX = (double)DpiChecker.GetDpiFromRect(rect).X / (double)DpiUtil.GetSystemDpi().X;
            double monitorScaleFactorY = (double)DpiChecker.GetDpiFromRect(rect).Y / (double)DpiUtil.GetSystemDpi().Y;
            ScaleTransform scaleTransform = new ScaleTransform(monitorScaleFactorX, monitorScaleFactorY);
            return scaleTransform;
        }

        public static double GetAbsoluteScaleFactorFromRect(Rect rect)
        {
            double scaleFactor = (double)DpiChecker.GetDpiFromRect(rect).X / (double)Dpi.Default.X;
            return scaleFactor;
        }

        /// <summary>
        /// WPF wants to snap sizes to values that will map to physical pixels on the monitor. This method will adjust a size to be a multiple of what 1 pixel would be in system DPI WPF sizes. 
        /// </summary>
        /// <example>For instance, at 175% scale factor 1 pixel = 1/1.75 = 0.571 WPF units, so we need to snap to a multiple of 0.571</example>
        /// <param name="window"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static double GetNearestMultipleSize(Window window, double size)
        {
            double sizeOnMonitor = ConvertSizeSystemDpiToRectDpi(new Rect(window.Left, window.Top, window.Width, window.Height), size);
            double stepsToSnapTo = ConvertSizePhysicalPixelToWpf(1);

            double nearestMultiple = Util.NearestMultiple(sizeOnMonitor, stepsToSnapTo);

            return nearestMultiple;
        }

        /// <summary>
        /// Gets the WPF size adjustment to make the output scaled resolution work for OBS (divisible by 4)
        /// </summary>
        /// <param name="rect">Rect used to determine scale factor. Should be on the screen that we want to get the DPI for.</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="shrinkOnly"></param>
        /// <returns></returns>
        public static Point GetWpfSizeAdjustmentForObs(Rect rect, double width, double height, bool shrinkOnly)
        {
            Debug.WriteLine("-----");
            double physicalOnScreenWidth = (double)ConvertSizeWpfToPhysicalPixel(width);
            double physicalOnScreenHeight = (double)ConvertSizeWpfToPhysicalPixel(height);
            Debug.WriteLine($"physicalOnScreenWidth: {physicalOnScreenWidth}, physicalOnScreenHeight: {physicalOnScreenHeight}");

            double monitorScaleFactor = GetAbsoluteScaleFactorFromRect(rect);
            double outputWidth = physicalOnScreenWidth / monitorScaleFactor;
            double outputHeight = physicalOnScreenHeight / monitorScaleFactor;
            Debug.WriteLine($"outputWidth: {outputWidth}, outputHeight: {outputHeight}");

            System.Windows.Point obsAdj = Util.GetObsResolutionAdjustment(outputWidth, outputHeight, shrinkOnly);
            Debug.WriteLine($"obsAdj.X: {obsAdj.X}, obsAdj.Y: {obsAdj.Y}");

            double obsAdjWpfX = ConvertSizeSystemDpiToRectDpi(rect, ConvertSizePhysicalPixelToWpf(obsAdj.X));
            double obsAdjWpfY = ConvertSizeSystemDpiToRectDpi(rect, ConvertSizePhysicalPixelToWpf(obsAdj.Y));
            Debug.WriteLine($"obsAdjWpfX: {obsAdjWpfX}, obsAdjWpfY: {obsAdjWpfY}");

            return new Point(obsAdjWpfX, obsAdjWpfY);
        }


    }
}