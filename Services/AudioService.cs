using OBS;
using obs_cli.Data;
using obs_cli.Objects;
using obs_cli.Objects.Obs;
using obs_cli.Structs;
using obs_cli.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web.Script.Serialization;
using vidgrid_recorder_data;
using static OBS.libobs;

namespace obs_cli.Services
{
    public static class AudioService
    {
        public static bool IsAudioInputCallbackEnabled { get; set; }
        public static bool IsAudioOutputCallbackEnabled { get; set; }

        /// <summary>
        /// Gets a list of all of the audio input devices.
        /// </summary>
        /// <returns></returns>
        public static List<AudioDevice> GetAudioInputDevices()
        {
            return GetAudioDevices(Store.Data.Audio.InputSource, Constants.Audio.Defaults.AudioInputName);
        }

        /// <summary>
        /// Gets a list of all of the audio output devices.
        /// </summary>
        /// <returns></returns>
        public static List<AudioDevice> GetAudioOutputDevices()
        {
            return GetAudioDevices(Store.Data.Audio.OutputSource, Constants.Audio.Defaults.AudioOutputName);
        }

        /// <summary>
        /// Updates the current audio input device.
        /// </summary>
        /// <param name="deviceId"></param>
        public static void UpdateAudioInput(string deviceId)
        {
            Store.Data.Audio.CurrentInputId = deviceId;

            ObsData aiSettings = new ObsData();
            aiSettings.SetString(Constants.Audio.SettingKeys.DeviceId, deviceId.Equals(Constants.Audio.NO_DEVICE_ID) ? Constants.Audio.DEFAULT_DEVICE_ID : deviceId);
            Store.Data.Audio.InputSource.Update(aiSettings);
            aiSettings.Dispose();

            Store.Data.Audio.InputSource.Enabled = !deviceId.Equals(Constants.Audio.NO_DEVICE_ID);
            Store.Data.Audio.InputSource.Muted = deviceId.Equals(Constants.Audio.NO_DEVICE_ID); // Muted is used to update audio meter

            WebcamService.UpdateAudioDevice();
        }

        /// <summary>
        /// Updates the current audio output device.
        /// </summary>
        /// <param name="deviceId"></param>
        public static void UpdateAudioOutput(string deviceId)
        {
            Store.Data.Audio.CurrentOutputId = deviceId;

            ObsData aoSettings = new ObsData();
            aoSettings.SetString(Constants.Audio.SettingKeys.DeviceId, deviceId.Equals(Constants.Audio.NO_DEVICE_ID) ? Constants.Audio.DEFAULT_DEVICE_ID : deviceId);
            Store.Data.Audio.OutputSource.Update(aoSettings);
            aoSettings.Dispose();

            Store.Data.Audio.OutputSource.Enabled = !deviceId.Equals(Constants.Audio.NO_DEVICE_ID);
            Store.Data.Audio.OutputSource.Muted = deviceId.Equals(Constants.Audio.NO_DEVICE_ID); // Muted is used to update audio meter
        }

        /// <summary>
        /// Resets and updates the audio settings for audio output.
        /// </summary>
        /// <returns></returns>
        public static bool ResetAudioInfo()
        {
            obs_audio_info avi = new obs_audio_info
            {
                samples_per_sec = Constants.Audio.SAMPLES_PER_SEC,
                speakers = speaker_layout.SPEAKERS_STEREO
            };

            if (!Obs.ResetAudio(avi))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sets the audio input to the device with the given audio input id.
        /// </summary>
        /// <param name="savedAudioInputId"></param>
        /// <returns></returns>
        public static string SetAudioInput(string savedAudioInputId)
        {
            ObsData aiSettings = new ObsData();
            aiSettings.SetBool(Constants.Audio.SettingKeys.UseDeviceTiming, false);
            Store.Data.Audio.InputSource = Store.Data.Obs.Presentation.CreateSource(Constants.Audio.SettingKeys.WasapiInputCapture, Constants.Audio.Settings.WasapiInputCaptureName, aiSettings);
            aiSettings.Dispose();

            Store.Data.Audio.InputSource.AudioOffset = Constants.Audio.DELAY_INPUT;
            Store.Data.Obs.Presentation.AddSource(Store.Data.Audio.InputSource);
            Store.Data.Audio.InputItem = Store.Data.Obs.Presentation.CreateItem(Store.Data.Audio.InputSource);
            Store.Data.Audio.InputItem.Name = Constants.Audio.Settings.WasapiInputCaptureName;

            Store.Data.Audio.InputMeter = new VolMeter();
            Store.Data.Audio.InputMeter.Level = float.NegativeInfinity;
            Store.Data.Audio.InputMeter.AttachSource(Store.Data.Audio.InputSource);
            Store.Data.Audio.InputMeter.AddCallBack(MagnitudeService.InputVolumeCallback);

            List<AudioDevice> allAudioInputs = GetAudioInputDevices();
            bool savedIsInAvailableInputs = allAudioInputs.Any(x => x.id == savedAudioInputId);

            var usedAudioInputId = string.Empty;

            if (savedAudioInputId != null && savedIsInAvailableInputs)
            {
                UpdateAudioInput(savedAudioInputId);
                usedAudioInputId = savedAudioInputId;
            }
            else
            {
                string defaultDeviceId = Constants.Audio.NO_DEVICE_ID;

                IEnumerable<AudioDevice> availableInputs = allAudioInputs.Where(x => x.id != Constants.Audio.NO_DEVICE_ID);
                if (availableInputs.Any())
                {
                    defaultDeviceId = availableInputs.First().id;
                }

                UpdateAudioInput(defaultDeviceId);
                usedAudioInputId = defaultDeviceId;
            }

            return usedAudioInputId;
        }

        /// <summary>
        /// Sets the audio output to the device with the given audio output id.
        /// </summary>
        /// <param name="savedAudioOutputId"></param>
        /// <returns></returns>
        public static string SetAudioOutput(string savedAudioOutputId)
        {
            ObsData aoSettings = new ObsData();
            aoSettings.SetBool(Constants.Audio.SettingKeys.UseDeviceTiming, false);
            Store.Data.Audio.OutputSource = Store.Data.Obs.Presentation.CreateSource(Constants.Audio.SettingKeys.WasapiOutputCapture, Constants.Audio.Settings.WasapiOutputCaptureName, aoSettings);
            aoSettings.Dispose();
            Store.Data.Audio.OutputSource.AudioOffset = Constants.Audio.DELAY_OUTPUT; // For some reason, this offset needs to be here before presentation.CreateSource is called again to take affect
            Store.Data.Obs.Presentation.AddSource(Store.Data.Audio.OutputSource);
            Store.Data.Audio.OutputItem = Store.Data.Obs.Presentation.CreateItem(Store.Data.Audio.OutputSource);
            Store.Data.Audio.OutputItem.Name = Constants.Audio.Settings.WasapiOutputCaptureName;

            Store.Data.Audio.OutputMeter = new VolMeter();
            Store.Data.Audio.OutputMeter.Level = float.NegativeInfinity;
            Store.Data.Audio.OutputMeter.AttachSource(Store.Data.Audio.OutputSource);
            Store.Data.Audio.OutputMeter.AddCallBack(MagnitudeService.OutputVolumeCallback);

            List<AudioDevice> allAudioOutputs = GetAudioOutputDevices();
            bool savedIsInAvailableOutputs = allAudioOutputs.Any(x => x.id == savedAudioOutputId);

            var usedAudioOutputId = string.Empty;

            if (savedAudioOutputId != null && savedIsInAvailableOutputs)
            {
                UpdateAudioOutput(savedAudioOutputId);
                usedAudioOutputId = savedAudioOutputId;
            }
            else
            {
                UpdateAudioOutput(Constants.Audio.NO_DEVICE_ID);
                usedAudioOutputId = Constants.Audio.NO_DEVICE_ID;
            }

            return usedAudioOutputId;
        }

        /// <summary>
        /// Gets all audio devices (input and output).
        /// </summary>
        /// <param name="audioSource"></param>
        /// <param name="defaultDeviceName"></param>
        /// <returns></returns>
        private static List<AudioDevice> GetAudioDevices(Source audioSource, string defaultDeviceName)
        {
            List<AudioDevice> audioDevices = new List<AudioDevice>();

            audioDevices.Add(new AudioDevice
            {
                name = Constants.Audio.Settings.AudioDeviceNoneName,
                id = Constants.Audio.NO_DEVICE_ID
            });

            ObsProperty[] audioSourceProperties = audioSource.GetProperties().GetPropertyList();
            for (int i = 0; i < audioSourceProperties.Length; i++)
            {
                if (audioSourceProperties[i].Name.Equals(Constants.Audio.SettingKeys.DeviceId))
                {
                    string[] propertyNames = audioSourceProperties[i].GetListItemNames();
                    object[] propertyValues = audioSourceProperties[i].GetListItemValues();

                    for (int j = 0; j < propertyNames.Length; j++)
                    {
                        string deviceName = propertyNames[j];
                        if (deviceName == Constants.Audio.Settings.DefaultDeviceName)
                        {
                            deviceName = defaultDeviceName;
                        }

                        AudioDevice device = new AudioDevice
                        {
                            name = deviceName,
                            id = (string)propertyValues[j]
                        };

                        audioDevices.Add(device);
                    }
                }
            }

            return audioDevices;
        }
    }
}
