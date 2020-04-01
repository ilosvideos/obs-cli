using OBS;
using obs_cli.Data;
using obs_cli.Objects;
using obs_cli.Structs;
using obs_cli.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace obs_cli.Services
{
    public static class AudioService
    {
        public const string DEFAULT_AUDIO_INPUT_DEVICE_NAME = "Primary Sound Capture Device";
        public const string DEFAULT_AUDIO_OUTPUT_DEVICE_NAME = "Primary Sound Output Device";

        /// <summary>
        /// Calculates the audio meter float value based on magnitude.
        /// </summary>
        /// <param name="magnitude"></param>
        /// <returns></returns>
        public static float CalculateAudioMeterLevel(float magnitude)
        {
            float level;

            if (magnitude <= -60)
            {
                level = 0.0f;
            }
            else if (magnitude >= -9)
            {
                level = 1.0f;
            }
            else
            {
                // 1.96 is 100/(60-9)
                level = (float)Math.Abs((-60 - magnitude) * (1.96) / 100);
            }

            return level;
        }

        /// <summary>
        /// Gets a list of all of the audio input devices.
        /// </summary>
        /// <returns></returns>
        public static List<AudioDevice> GetAudioInputDevices()
        {
            return GetAudioDevices(Store.Data.Audio.InputSource, DEFAULT_AUDIO_INPUT_DEVICE_NAME);
        }

        /// <summary>
        /// Gets a list of all of the audio output devices.
        /// </summary>
        /// <returns></returns>
        public static List<AudioDevice> GetAudioOutputDevices()
        {
            return GetAudioDevices(Store.Data.Audio.OutputSource, DEFAULT_AUDIO_OUTPUT_DEVICE_NAME);
        }

        /// <summary>
        /// Updates the current audio input device.
        /// </summary>
        /// <param name="deviceId"></param>
        public static void UpdateAudioInput(string deviceId)
        {
            Store.Data.Audio.CurrentInputId = deviceId;

            ObsData aiSettings = new ObsData();
            aiSettings.SetString("device_id", deviceId.Equals(Constants.Audio.NO_DEVICE_ID) ? Constants.Audio.DEFAULT_DEVICE_ID : deviceId);
            Store.Data.Audio.InputSource.Update(aiSettings);
            aiSettings.Dispose();

            Store.Data.Audio.InputSource.Enabled = !deviceId.Equals(Constants.Audio.NO_DEVICE_ID);
            Store.Data.Audio.InputSource.Muted = deviceId.Equals(Constants.Audio.NO_DEVICE_ID); // Muted is used to update audio meter

            // todo: webcam related
            //Webcam_UpdateAudioDevice();
        }

        /// <summary>
        /// Updates the current audio output device.
        /// </summary>
        /// <param name="deviceId"></param>
        public static void UpdateAudioOutput(string deviceId)
        {
            Store.Data.Audio.CurrentOutputId = deviceId;

            ObsData aoSettings = new ObsData();
            aoSettings.SetString("device_id", deviceId.Equals(Constants.Audio.NO_DEVICE_ID) ? Constants.Audio.DEFAULT_DEVICE_ID : deviceId);
            Store.Data.Audio.OutputSource.Update(aoSettings);
            aoSettings.Dispose();

            Store.Data.Audio.OutputSource.Enabled = !deviceId.Equals(Constants.Audio.NO_DEVICE_ID);
            Store.Data.Audio.OutputSource.Muted = deviceId.Equals(Constants.Audio.NO_DEVICE_ID); // Muted is used to update audio meter
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
                name = "None",
                id = Constants.Audio.NO_DEVICE_ID
            });

            ObsProperty[] audioSourceProperties = audioSource.GetProperties().GetPropertyList();
            for (int i = 0; i < audioSourceProperties.Length; i++)
            {
                if (audioSourceProperties[i].Name.Equals("device_id"))
                {
                    string[] propertyNames = audioSourceProperties[i].GetListItemNames();
                    object[] propertyValues = audioSourceProperties[i].GetListItemValues();

                    for (int j = 0; j < propertyNames.Length; j++)
                    {
                        string deviceName = propertyNames[j];
                        if (deviceName == "Default")
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
