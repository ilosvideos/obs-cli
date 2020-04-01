using OBS;
using obs_cli.Data;
using obs_cli.Helpers;
using obs_cli.Objects;
using obs_cli.Structs;
using obs_cli.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static OBS.libobs;

namespace obs_cli.Commands.Implementations
{
    // todo: need to do InitPresentation logic still
    public class Initialize : ICommand
    {
        public int CropTop { get; set; }        
        public int CropRight { get; set; }
        public int CropLeft { get; set; }
        public int CropBottom { get; set; }
        public uint FrameRate { get; set; }
        public double OutputWidth { get; set; }
        public double OutputHeight { get; set; }
        public int CanvasWidth { get; set; }
        public int CanvasHeight { get; set; }
        public IntPtr ScreenToRecordHandle { get; set; }
        public string SavedAudioInputId { get; set; }
        public string SavedAudioOutputId { get; set; }

        // todo: these need to go somewhere else because they might be accessed by multiple commands.
        // maybe make a class that encapsulates all of the logic in managing them and then have a single static instance of that class?
        public Presentation Presentation;
        public Scene MainScene;
        public Scene WebcamScene;

        public obs_sceneitem_crop AppliedCrop;

        public static string Name
        {
            get
            {
                return "initialize";
            }
        }

        public Initialize(IDictionary<string, string> arguments)
        {
            FileWriteService.WriteToFile($"got {arguments.Count} arguments");

            this.CropTop = int.Parse(arguments["cropTop"]);
            this.CropRight = int.Parse(arguments["cropRight"]);
            this.CropBottom = int.Parse(arguments["cropBottom"]);
            this.CropLeft = int.Parse(arguments["cropLeft"]);
            this.FrameRate = uint.Parse(arguments["frameRate"]);
            this.CanvasWidth = int.Parse(arguments["canvasWidth"]);
            this.CanvasHeight = int.Parse(arguments["canvasHeight"]);
            this.OutputWidth = double.Parse(arguments["outputWidth"]);
            this.OutputHeight = double.Parse(arguments["outputHeight"]);
            this.ScreenToRecordHandle = (IntPtr)int.Parse(arguments["screenToRecordHandle"]);
            this.SavedAudioInputId = arguments["savedAudioInputId"];
            this.SavedAudioOutputId = arguments["savedAudioOutputId"];

            FileWriteService.WriteToFile($"Initializing with CropTop: {CropTop}");
            FileWriteService.WriteToFile($"Initializing with CropRight: {CropRight}");
            FileWriteService.WriteToFile($"Initializing with CropBottom: {CropBottom}");
            FileWriteService.WriteToFile($"Initializing with CropLeft: {CropLeft}");

            FileWriteService.WriteToFile($"Initializing with FrameRate: {FrameRate}");

            FileWriteService.WriteToFile($"Initializing with CanvasWidth: {CanvasWidth}");
            FileWriteService.WriteToFile($"Initializing with CanvasHeight: {CanvasHeight}");
            FileWriteService.WriteToFile($"Initializing with OutputWidth: {OutputWidth}");
            FileWriteService.WriteToFile($"Initializing with OutputHeight: {OutputHeight}");

            FileWriteService.WriteToFile($"Received screen handle: {ScreenToRecordHandle}");
        }

        public void Execute()
        {
            FileWriteService.WriteToFile("initialize command start");

            if (!Obs.Startup("en-US"))
            {
                // todo: if any exceptions are thrown in this app, we need to bubble it all up to a single terminate code so consuming apps know that it shut down
                throw new ApplicationException("Startup failed.");
            }

            ResetAudioInfo();

            FileWriteService.WriteToFile("ResetAudioInfo successful");

            ResetVideoInfo();

            FileWriteService.WriteToFile("ResetVideoInfo successful");

            Obs.LoadAllModules();

            FileWriteService.WriteToFile("Obs.LoadAllModules successful");

            FileWriteService.WriteToFile("initialize command end");

            Presentation = new Presentation();
            MainScene = Presentation.AddScene("Main");
            WebcamScene = Presentation.AddScene("Webcam");
            Presentation.SetScene(MainScene);

            Store.Data.Display.DisplaySource = Presentation.CreateSource("monitor_capture", "Monitor Capture Source");
            Presentation.AddSource(Store.Data.Display.DisplaySource);
            Store.Data.Display.DisplayItem = Presentation.CreateItem(Store.Data.Display.DisplaySource);
            Store.Data.Display.DisplayItem.Name = "Monitor Capture SceneItem";

            Rectangle activeScreenBounds = ScreenHelper.GetScreen(this.ScreenToRecordHandle).Bounds;

            Store.Data.Display.DisplayItem.SetBounds(new Vector2(activeScreenBounds.Width, activeScreenBounds.Height), ObsBoundsType.None, ObsAlignment.Top); // this should always be the screen's resolution
            MainScene.Items.Add(Store.Data.Display.DisplayItem);

            SetAudioInput();

            SetAudioOutput();

            Presentation.SetItem(0);
            Presentation.SetSource(0);
        }
        private void SetAudioInput()
        {
            ObsData aiSettings = new ObsData();
            aiSettings.SetBool("use_device_timing", false);
            Store.Data.Audio.InputSource = Presentation.CreateSource("wasapi_input_capture", "Mic", aiSettings);
            aiSettings.Dispose();

            Store.Data.Audio.InputSource.AudioOffset = Constants.Audio.DELAY_INPUT;
            Presentation.AddSource(Store.Data.Audio.InputSource);
            Store.Data.Audio.InputItem = Presentation.CreateItem(Store.Data.Audio.InputSource);
            Store.Data.Audio.InputItem.Name = "Mic";

            Store.Data.Audio.InputMeter = new VolMeter();
            Store.Data.Audio.InputMeter.AttachSource(Store.Data.Audio.InputSource);
            Store.Data.Audio.InputMeter.AddCallBack(InputVolumeCallback);

            // not sure what to do with this yet?
            string savedAudioInputId = this.SavedAudioInputId;

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

        private void SetAudioOutput()
        {
            ObsData aoSettings = new ObsData();
            aoSettings.SetBool("use_device_timing", false);
            Store.Data.Audio.OutputSource = Presentation.CreateSource("wasapi_output_capture", "Desktop Audio", aoSettings);
            aoSettings.Dispose();
            Store.Data.Audio.OutputSource.AudioOffset = Constants.Audio.DELAY_OUTPUT; // For some reason, this offset needs to be here before presentation.CreateSource is called again to take affect
            Presentation.AddSource(Store.Data.Audio.OutputSource);
            Store.Data.Audio.OutputItem = Presentation.CreateItem(Store.Data.Audio.OutputSource);
            Store.Data.Audio.OutputItem.Name = "Desktop Audio";

            Store.Data.Audio.OutputMeter = new VolMeter();
            Store.Data.Audio.OutputMeter.AttachSource(Store.Data.Audio.OutputSource);
            Store.Data.Audio.OutputMeter.AddCallBack(OutputVolumeCallback);

            string savedAudioOutputId = this.SavedAudioOutputId;
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
        public void InputVolumeCallback(IntPtr data, float[] magnitude, float[] peak, float[] input_peak)
        {
            Store.Data.Audio.InputMeter.Level = CalculateAudioMeterLevel(magnitude[0]);
        }

        public void OutputVolumeCallback(IntPtr data, float[] magnitude, float[] peak, float[] input_peak)
        {
            Store.Data.Audio.OutputMeter.Level = CalculateAudioMeterLevel(magnitude[0]);
        }

        public void UpdateAudioInput(string deviceId)
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

        public void UpdateAudioOutput(string deviceId)
        {
            Store.Data.Audio.CurrentOutputId = deviceId;

            ObsData aoSettings = new ObsData();
            aoSettings.SetString("device_id", deviceId.Equals(Constants.Audio.NO_DEVICE_ID) ? Constants.Audio.DEFAULT_DEVICE_ID : deviceId);
            Store.Data.Audio.OutputSource.Update(aoSettings);
            aoSettings.Dispose();

            Store.Data.Audio.OutputSource.Enabled = !deviceId.Equals(Constants.Audio.NO_DEVICE_ID);
            Store.Data.Audio.OutputSource.Muted = deviceId.Equals(Constants.Audio.NO_DEVICE_ID); // Muted is used to update audio meter
        }

        private float CalculateAudioMeterLevel(float magnitude)
        {
            float level = 0.0f;

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

        private void ResetAudioInfo()
        {
            obs_audio_info avi = new obs_audio_info
            {
                samples_per_sec = Constants.Audio.SAMPLES_PER_SEC,
                speakers = speaker_layout.SPEAKERS_STEREO
            };

            if (!Obs.ResetAudio(avi))
                throw new ApplicationException("ResetAudio failed.");
        }

        private uint GetFrameRate()
        {
            int minfps = 4;
            int maxfps = 30;
            int fps = (int)FrameRate;

            if (fps < 4)
            {
                fps = minfps;
            }
            else if (fps > maxfps)
            {
                fps = maxfps;
            }

            return (uint)fps;
        }

        private void ResetVideoInfo()
        {
            if (Presentation != null)
            {
                if (Presentation.SelectedScene.GetName().ToLowerInvariant() != "main")
                {
                    Presentation.SetScene(0);
                }
            }

            AppliedCrop = new obs_sceneitem_crop
            {
                left = CropLeft,
                top = CropTop,
                right = CropRight,
                bottom = CropBottom
            };

            //Set the proper display source
            if (Store.Data.Display.DisplaySource != null)
            {
                ObsData displaySettings = new ObsData();
                displaySettings.SetBool("capture_cursor", true);
                displaySettings.SetInt("monitor", ObsHelper.GetObsDisplayValueFromScreen(Store.Data.Display.DisplaySource, ScreenHelper.GetScreen(this.ScreenToRecordHandle)));
                Store.Data.Display.DisplaySource.Update(displaySettings);
                displaySettings.Dispose();
            }

            //Set the proper display bounds and crop
            if (Store.Data.Display.DisplayItem != null)
            {
                Store.Data.Display.DisplayItem.SetBounds(new Vector2(0, 0), ObsBoundsType.None, ObsAlignment.Top);
                Store.Data.Display.DisplayItem.SetCrop(AppliedCrop);
            }

            // todo: webcam related
            //CalculateWebcamItemPosition();

            obs_video_info ovi = ObsHelper.GenerateObsVideoInfoObject(
                (uint)CanvasWidth,
                (uint)CanvasHeight,
                (uint)OutputWidth,
                (uint)OutputHeight,
                GetFrameRate());

            if (!Obs.ResetVideo(ovi))
                throw new ApplicationException("ResetVideo failed.");
        }
        public List<AudioDevice> GetAudioInputDevices()
        {
            return GetAudioDevices(Store.Data.Audio.InputSource, "Primary Sound Capture Device");
        }

        public List<AudioDevice> GetAudioOutputDevices()
        {
            return GetAudioDevices(Store.Data.Audio.OutputSource, "Primary Sound Output Device");
        }

        private List<AudioDevice> GetAudioDevices(Source audioSource, string defaultDeviceName)
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
