using DirectShowLib;
using Microsoft.Win32;
using OBS;
using obs_cli.Data;
using obs_cli.Helpers;
using obs_cli.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace obs_cli.Services
{
	public static class WebcamService
    {
		/// <summary>
		/// Determines what the best webcam resolution for the given device is.
		/// </summary>
		/// <param name="deviceValue"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Finds DirectShow audio device from current wsapi audio device to attach to the webcam.
		/// </summary>
		public static void UpdateAudioDevice()
		{
			// DISABLING CUSTOM AUDIO DEVICE - if re-enabling, search for this comment
			if (Store.Data.Audio.InputSource != null)
			{
				Store.Data.Audio.InputSource.AudioOffset = Constants.Audio.DELAY_INPUT_NOT_ATTACHED_TO_WEBCAM;
			}

			return;

			if (Store.Data.Webcam.Source == null)
			{
				return;
			}

			// Disable the main microphone, we need to attach it to the webcamsource_webcam
			Store.Data.Audio.InputSource.Enabled = false;

			var foundWebcamAudio = false;

			if (Store.Data.Audio.CurrentInputId == Constants.Audio.NO_DEVICE_ID)
			{
				ObsData wcSettings = new ObsData();
				wcSettings.SetBool("use_custom_audio_device", false);
				Store.Data.Webcam.Source.Update(wcSettings);
				foundWebcamAudio = true;
				return;
			}

			Store.Data.Webcam.Source.GetProperties();

			string devenumDir = FX35Helper.Is64BitProcess() ? "devenum 64-bit" : "devenum";
			RegistryKey inputDevicesDirectory = Registry.CurrentUser.OpenSubKey("SOFTWARE")?.OpenSubKey("Microsoft")?.OpenSubKey("ActiveMovie")?.OpenSubKey(devenumDir)?.OpenSubKey("{33D9A762-90C8-11D0-BD43-00A0C911CE86}");

			if (inputDevicesDirectory == null)
			{
				foundWebcamAudio = false;
				return;
			}

			string[] subKeyNames = inputDevicesDirectory.GetSubKeyNames();
			string directShowDeviceId = null;

			foreach (string name in subKeyNames)
			{
				RegistryKey subkey = inputDevicesDirectory.OpenSubKey(name);
				string endpointId = subkey.GetValue("EndpointId")?.ToString();
				object waveInId = subkey.GetValue("WaveInId");

				if (Store.Data.Audio.CurrentInputId == Constants.Audio.DEFAULT_DEVICE_ID)
				{
					if (waveInId == null || (int)waveInId != 0) continue;
				}
				else if (endpointId == null || endpointId != Store.Data.Audio.CurrentInputId)
				{
					continue;
				}

				string friendlyName = subkey.GetValue("FriendlyName")?.ToString();
				if (friendlyName != null)
				{
					// Direct show adds a colon to the friendly name for the id
					directShowDeviceId = $"{friendlyName}:";
					break;
				}
			}

			if (directShowDeviceId == null)
			{
				foundWebcamAudio = false;
				return;
			}

			ObsData wSettings = new ObsData();
			wSettings.SetBool("use_custom_audio_device", true);
			wSettings.SetString("audio_device_id", directShowDeviceId);
			Store.Data.Webcam.Source.Update(wSettings);
			foundWebcamAudio = true;
		}
    }
}
