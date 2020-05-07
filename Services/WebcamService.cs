using DirectShowLib;
using obs_cli.Helpers;
using obs_cli.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace obs_cli.Services
{
    public class WebcamService
    {
        public static string GetOptimalWebcamResolution(string deviceValue)
        {
            string preferredResolution = "";

            try
            {
                // Try to find a preferred resolution. This lets us choose the preferred resolution and also fixes some initialization issues with some webcams.                    
                DsDevice[] devices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
                if (devices.Length == 0)
                {
                    throw new Exception("Could not find any video input devices");
                }

                IEnumerable<DsDevice> videoInputDevices = devices.Where(x => x.DevicePath.Contains(deviceValue));
                if (!videoInputDevices.Any())
                {
                    throw new Exception($"Could not find video input device: {deviceValue}");
                }

                DsDevice videoInputDevice = videoInputDevices.First();
                List<Size> allDeviceRes = DShowUtility.GetAllAvailableResolutions(videoInputDevice);
                if (!allDeviceRes.Any())
                {
                    throw new Exception($"Could not find resolutions for device: {deviceValue}");
                }

                if (allDeviceRes.Any(x => x.Width == 1280 && x.Height == 720))
                {
                    preferredResolution = "1280x720";
                }
                else if (allDeviceRes.Any(x => x.Width == 1920 && x.Height == 1080))
                {
                    preferredResolution = "1920x1080";
                }
                else if (allDeviceRes.Any(x => x.Width < 1280))
                {
                    Size preferredRes = allDeviceRes.First(x => x.Width < 1280);
                    preferredResolution = $"{preferredRes.Width}x{preferredRes.Height}";
                }
                else
                {
                    Size preferredRes = allDeviceRes.First();
                    preferredResolution = $"{preferredRes.Width}x{preferredRes.Height}";
                }
            }
            catch (Exception)
            {
                FileWriteService.WriteLineToFile("Exception thrown while searching for optimal webcam resolution");
            }

            return preferredResolution;
        }
    }
}
