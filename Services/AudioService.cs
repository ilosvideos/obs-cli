using OBS;
using obs_cli.Data;
using obs_cli.Objects;
using obs_cli.Objects.Obs;
using obs_cli.Structs;
using obs_cli.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using static OBS.libobs;

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
        public static void SetAudioInput(string savedAudioInputId)
        {
            ObsData aiSettings = new ObsData();
            aiSettings.SetBool("use_device_timing", false);
            Store.Data.Audio.InputSource = Store.Data.Obs.Presentation.CreateSource("wasapi_input_capture", "Mic", aiSettings);
            aiSettings.Dispose();

            Store.Data.Audio.InputSource.AudioOffset = Constants.Audio.DELAY_INPUT;
            Store.Data.Obs.Presentation.AddSource(Store.Data.Audio.InputSource);
            Store.Data.Audio.InputItem = Store.Data.Obs.Presentation.CreateItem(Store.Data.Audio.InputSource);
            Store.Data.Audio.InputItem.Name = "Mic";

            Store.Data.Audio.InputMeter = new VolMeter();
            Store.Data.Audio.InputMeter.AttachSource(Store.Data.Audio.InputSource);
            Store.Data.Audio.InputMeter.AddCallBack(InputVolumeCallback);

            List<AudioDevice> allAudioInputs = GetAudioInputDevices();
            bool savedIsInAvailableInputs = allAudioInputs.Any(x => x.id == savedAudioInputId);

            if (savedAudioInputId != null && savedIsInAvailableInputs)
            {
                UpdateAudioInput(savedAudioInputId);
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
            }
        }

        /// <summary>
        /// Sets the audio output to the device with the given audio output id. 
        /// </summary>
        /// <param name="savedAudioOutputId"></param>
        public static void SetAudioOutput(string savedAudioOutputId)
        {
            ObsData aoSettings = new ObsData();
            aoSettings.SetBool("use_device_timing", false);
            Store.Data.Audio.OutputSource = Store.Data.Obs.Presentation.CreateSource("wasapi_output_capture", "Desktop Audio", aoSettings);
            aoSettings.Dispose();
            Store.Data.Audio.OutputSource.AudioOffset = Constants.Audio.DELAY_OUTPUT; // For some reason, this offset needs to be here before presentation.CreateSource is called again to take affect
            Store.Data.Obs.Presentation.AddSource(Store.Data.Audio.OutputSource);
            Store.Data.Audio.OutputItem = Store.Data.Obs.Presentation.CreateItem(Store.Data.Audio.OutputSource);
            Store.Data.Audio.OutputItem.Name = "Desktop Audio";

            Store.Data.Audio.OutputMeter = new VolMeter();
            Store.Data.Audio.OutputMeter.AttachSource(Store.Data.Audio.OutputSource);
            Store.Data.Audio.OutputMeter.AddCallBack(EmitOutputMagnitude);

            List<AudioDevice> allAudioOutputs = GetAudioOutputDevices();
            bool savedIsInAvailableOutputs = allAudioOutputs.Any(x => x.id == savedAudioOutputId);

            if (savedAudioOutputId != null && savedIsInAvailableOutputs)
            {
                UpdateAudioOutput(savedAudioOutputId);
            }
            else
            {
                UpdateAudioOutput(Constants.Audio.NO_DEVICE_ID);
            }
        }

        // As of OBS 21.0.1, audo meters have been reworked. We now need to calculate and draw ballistics ourselves. 
        // Relevant commit: https://github.com/obsproject/obs-studio/commit/50ce2284557b888f230a1730fa580e82a6a133dc#diff-505cedf4005a973efa8df1e299be4199
        // This is probably an over-simplified calculation.
        // For practical purposes, we are treating -60 as 0 and -9 as 1.
        private static void InputVolumeCallback(IntPtr data, float[] magnitude, float[] peak, float[] input_peak)
        {
            EmitService.EmitInputMagnitude(new AudioMagnitudeParameters { Magnitude = magnitude[0] });
        }

        private static void EmitOutputMagnitude(IntPtr data, float[] magnitude, float[] peak, float[] input_peak)
        {
            EmitService.EmitOutputMagnitude(new AudioMagnitudeParameters { Magnitude = magnitude[0] });
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
