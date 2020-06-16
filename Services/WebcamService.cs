using DirectShowLib;
using Microsoft.Win32;
using OBS;
using obs_cli.Data;
using obs_cli.Data.Modules;
using obs_cli.Structs;
using obs_cli.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WebcamDevice = obs_cli.Objects.Webcam;

namespace obs_cli.Services
{
	public static class WebcamService
    {
		/// <summary>
		/// Calculates where the OBS webcam item should be positioned.
		/// </summary>
		public static void CalculateItemPosition()
		{
			if (Store.Data.Webcam.Item == null)
			{
				return;
			}

			// todo: verify that this is the correct screen
			Rect activeScreenBounds = DpiUtil.GetScreenWpfBounds(Store.Data.Record.ActiveScreen);

			double baseOffsetX = activeScreenBounds.X;
			double baseOffsetY = activeScreenBounds.Y;

			if (!Store.Data.Record.IsFullScreen)
			{
				baseOffsetX = Store.Data.Record.AppliedCrop.left;
				baseOffsetY = Store.Data.Record.AppliedCrop.top;
			}

			WindowSizeProperties webcamWindowSize = Util.GetWindowSizeProperties(Store.Data.Webcam.Window);
			double webcamItemWidth = webcamWindowSize.Width - (webcamWindowSize.BorderWidth.Value * 2);
			double webcamItemHeight = webcamWindowSize.Height - (webcamWindowSize.BorderWidth.Value * 2);

			int webcamObsX = DpiUtil.ConvertSizeWpfToPhysicalPixel(webcamWindowSize.Left + webcamWindowSize.BorderWidth.Value - baseOffsetX);
			int webcamObsY = DpiUtil.ConvertSizeWpfToPhysicalPixel(webcamWindowSize.Top + webcamWindowSize.BorderWidth.Value - baseOffsetY);
			int webcamObsWidth = DpiUtil.ConvertSizeWpfToPhysicalPixel(webcamItemWidth);
			int webcamObsHeight = DpiUtil.ConvertSizeWpfToPhysicalPixel(webcamItemHeight);

			Store.Data.Webcam.Item.Position = new Vector2((float)webcamObsX, (float)webcamObsY);
			Store.Data.Webcam.Item.SetBounds(new Vector2(webcamObsWidth, webcamObsHeight), ObsBoundsType.ScaleOuter, ObsAlignment.Center);

			double webcamAspectRatio = Convert.ToDouble(Store.Data.Webcam.Source.Width) / Convert.ToDouble(Store.Data.Webcam.Source.Height);
			double webcamItemAspectRatio = Convert.ToDouble(webcamItemWidth) / Convert.ToDouble(webcamItemHeight);
			double webcamWidthD = Convert.ToDouble(Store.Data.Webcam.Source.Width);
			double webcamHeightD = Convert.ToDouble(Store.Data.Webcam.Source.Height);
			int cropWidth = webcamItemAspectRatio < webcamAspectRatio ? Convert.ToInt32((webcamWidthD - (webcamHeightD * webcamItemAspectRatio)) / 2) : 0;
			int cropHeight = webcamItemAspectRatio < webcamAspectRatio ? 0 : Convert.ToInt32((webcamHeightD - (webcamWidthD / webcamItemAspectRatio)) / 2);
			libobs.obs_sceneitem_crop webcamCrop = new libobs.obs_sceneitem_crop
			{
				left = webcamItemAspectRatio < webcamAspectRatio ? cropWidth : 0,
				top = webcamItemAspectRatio < webcamAspectRatio ? 0 : cropHeight,
				right = webcamItemAspectRatio < webcamAspectRatio ? cropWidth : 0,
				bottom = webcamItemAspectRatio < webcamAspectRatio ? 0 : cropHeight
			};

			Store.Data.Webcam.Item.SetCrop(webcamCrop);
		}

		/// <summary>
		/// Disposes the OBS webcam item and source and reenables standalone audio input.
		/// </summary>
		public static void DestroyObsWebcam()
		{
			Store.Data.Obs.MainScene.Items.Remove(Store.Data.Webcam.Item);
			Store.Data.Webcam.Item.Remove();
			Store.Data.Webcam.Item.Dispose();
			Store.Data.Webcam.Item = null;

			Store.Data.Obs.Presentation.Sources.Remove(Store.Data.Webcam.Source);
			Store.Data.Webcam.Source.Enabled = false;
			Store.Data.Webcam.Source.Remove();
			Store.Data.Webcam.Source.Dispose();
			Store.Data.Webcam.Source = null;

			Store.Data.Audio.InputSource.Enabled = true;
			Store.Data.Audio.InputSource.AudioOffset = Constants.Audio.DELAY_INPUT;

			Store.Data.Webcam.WebcamSettings = new WebcamSettings();
		}

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
			catch (Exception ex)
			{
				//FileWriteService.WriteLineToFile(Constants.Webcam.ErrorMessages.OptimalResolutionNotFound);
				throw ex;
			}

			return preferredResolution;
		}

		/// <summary>
		/// Gets the webcam object with the corresponding value.
		/// </summary>
		/// <param name="webcamValue"></param>
		/// <returns></returns>
		public static WebcamDevice GetWebcam(string webcamValue)
		{
			var value = webcamValue;
			if (webcamValue.IndexOf(Constants.Webcam.Settings.NameValueDelimiter) >= 0)
			{
				value = webcamValue.Substring(webcamValue.IndexOf(Constants.Webcam.Settings.NameValueDelimiter));
			}

			return Store.Data.Webcam.Webcams.FirstOrDefault(x => x.value.IndexOf(value) >= 0);
		}

		/// <summary>
		/// Moves the OBS webcam item off screen.
		/// </summary>
		public static void SetWebcamItemOffscreen()
		{
			Store.Data.Webcam.Item.Position = new Vector2(Store.Data.Record.CanvasWidth, Store.Data.Record.CanvasHeight);
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
