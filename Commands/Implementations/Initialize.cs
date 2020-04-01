using OBS;
using obs_cli.Data;
using obs_cli.Helpers;
using obs_cli.Objects;
using obs_cli.Services;
using obs_cli.Structs;
using obs_cli.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static OBS.libobs;

namespace obs_cli.Commands.Implementations
{
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

        public static string Name
        {
            get
            {
                return "initialize";
            }
        }

        public Initialize(IDictionary<string, string> arguments)
        {
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

            Store.Data.Obs.Presentation = new Presentation();
            Store.Data.Obs.MainScene = Store.Data.Obs.Presentation.AddScene("Main");
            Store.Data.Obs.WebcamScene = Store.Data.Obs.Presentation.AddScene("Webcam");
            Store.Data.Obs.Presentation.SetScene(Store.Data.Obs.MainScene);

            Store.Data.Display.DisplaySource = Store.Data.Obs.Presentation.CreateSource("monitor_capture", "Monitor Capture Source");
            Store.Data.Obs.Presentation.AddSource(Store.Data.Display.DisplaySource);
            Store.Data.Display.DisplayItem = Store.Data.Obs.Presentation.CreateItem(Store.Data.Display.DisplaySource);
            Store.Data.Display.DisplayItem.Name = "Monitor Capture SceneItem";

            Rectangle activeScreenBounds = ScreenHelper.GetScreen(this.ScreenToRecordHandle).Bounds;

            Store.Data.Display.DisplayItem.SetBounds(new Vector2(activeScreenBounds.Width, activeScreenBounds.Height), ObsBoundsType.None, ObsAlignment.Top); // this should always be the screen's resolution
            Store.Data.Obs.MainScene.Items.Add(Store.Data.Display.DisplayItem);

            SetAudioInput();

            SetAudioOutput();

            Store.Data.Obs.Presentation.SetItem(0);
            Store.Data.Obs.Presentation.SetSource(0);

            FileWriteService.WriteToFile("initialize command end");
        }

        private void SetAudioInput()
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

            string savedAudioInputId = this.SavedAudioInputId;

            List<AudioDevice> allAudioInputs = AudioService.GetAudioInputDevices();
            bool savedIsInAvailableInputs = allAudioInputs.Any(x => x.id == savedAudioInputId);

            if (savedAudioInputId != null && savedIsInAvailableInputs)
            {
                AudioService.UpdateAudioInput(savedAudioInputId);
            }
            else
            {
                string defaultDeviceId = Constants.Audio.NO_DEVICE_ID;

                IEnumerable<AudioDevice> availableInputs = allAudioInputs.Where(x => x.id != Constants.Audio.NO_DEVICE_ID);
                if (availableInputs.Any())
                {
                    defaultDeviceId = availableInputs.First().id;
                }

                AudioService.UpdateAudioInput(defaultDeviceId);
            }
        }

        private void SetAudioOutput()
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
            Store.Data.Audio.OutputMeter.AddCallBack(OutputVolumeCallback);

            string savedAudioOutputId = this.SavedAudioOutputId;
            List<AudioDevice> allAudioOutputs = AudioService.GetAudioOutputDevices();
            bool savedIsInAvailableOutputs = allAudioOutputs.Any(x => x.id == savedAudioOutputId);

            if (savedAudioOutputId != null && savedIsInAvailableOutputs)
            {
                AudioService.UpdateAudioOutput(savedAudioOutputId);
            }
            else
            {
                AudioService.UpdateAudioOutput(Constants.Audio.NO_DEVICE_ID);
            }
        }

        // As of OBS 21.0.1, audo meters have been reworked. We now need to calculate and draw ballistics ourselves. 
        // Relevant commit: https://github.com/obsproject/obs-studio/commit/50ce2284557b888f230a1730fa580e82a6a133dc#diff-505cedf4005a973efa8df1e299be4199
        // This is probably an over-simplified calculation.
        // For practical purposes, we are treating -60 as 0 and -9 as 1.
        private void InputVolumeCallback(IntPtr data, float[] magnitude, float[] peak, float[] input_peak)
        {
            Store.Data.Audio.InputMeter.Level = AudioService.CalculateAudioMeterLevel(magnitude[0]);
        }

        private void OutputVolumeCallback(IntPtr data, float[] magnitude, float[] peak, float[] input_peak)
        {
            Store.Data.Audio.OutputMeter.Level = AudioService.CalculateAudioMeterLevel(magnitude[0]);
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

        private void ResetVideoInfo()
        {
            if (Store.Data.Obs.Presentation != null)
            {
                if (Store.Data.Obs.Presentation.SelectedScene.GetName().ToLowerInvariant() != "main")
                {
                    Store.Data.Obs.Presentation.SetScene(0);
                }
            }

            Store.Data.Obs.AppliedCrop = new obs_sceneitem_crop
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
                Store.Data.Display.DisplayItem.SetCrop(Store.Data.Obs.AppliedCrop);
            }

            // todo: webcam related
            //CalculateWebcamItemPosition();

            obs_video_info ovi = ObsHelper.GenerateObsVideoInfoObject(
                (uint)CanvasWidth,
                (uint)CanvasHeight,
                (uint)OutputWidth,
                (uint)OutputHeight,
                VideoService.GetFrameRate(FrameRate));

            if (!Obs.ResetVideo(ovi))
                throw new ApplicationException("ResetVideo failed.");
        }
    }
}
