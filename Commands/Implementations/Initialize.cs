using OBS;
using obs_cli.Helpers;
using obs_cli.Objects;
using obs_cli.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
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

        // todo: these need to go somewhere else because they might be accessed by multiple commands.
        // maybe make a class that encapsulates all of the logic in managing them and then have a single static instance of that class?
        public Presentation Presentation;
        public obs_sceneitem_crop AppliedCrop;

        public Source DisplaySource;
        public Item DisplayItem;

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
            if (DisplaySource != null)
            {
                ObsData displaySettings = new ObsData();
                displaySettings.SetBool("capture_cursor", true);
                displaySettings.SetInt("monitor", GetObsDisplayValueFromScreen(ScreenHelper.GetScreen(this.ScreenToRecordHandle)));
                DisplaySource.Update(displaySettings);
                displaySettings.Dispose();
            }

            //Set the proper display bounds and crop
            if (DisplayItem != null)
            {
                DisplayItem.SetBounds(new Vector2(0, 0), ObsBoundsType.None, ObsAlignment.Top);
                DisplayItem.SetCrop(AppliedCrop);
            }

            //CalculateWebcamItemPosition();

            obs_video_info ovi = GenerateObsVideoInfoObject(
                (uint)CanvasWidth,
                (uint)CanvasHeight,
                (uint)OutputWidth,
                (uint)OutputHeight);

            if (!Obs.ResetVideo(ovi))
                throw new ApplicationException("ResetVideo failed.");
        }

        /** 
		 * Get the active display from the window's position.
		 * We have to do it this way because the OBS index (or display value) is different than that of Screen.AllScreens
		 */
        private int GetObsDisplayValueFromScreen(Screen screen)
        {
            // Get a list of OBS properties (names and values) from the display source
            ObsProperty[] displayCaptureProperties = DisplaySource.GetProperties().GetPropertyList();
            List<string> displayNames = new List<string>();
            List<object> displayValues = new List<object>();
            for (int i = 0; i < displayCaptureProperties.Length; i++)
            {
                if (displayCaptureProperties[i].Name.Equals("monitor"))
                {
                    displayNames = displayCaptureProperties[i].GetListItemNames().ToList();
                    displayValues = displayCaptureProperties[i].GetListItemValues().ToList();
                    break;
                }
            }

            // Find the OBS display that matches the bounds of our active screen. OBS display names are in the format of "Display {value}: WidthxHeight @ X,Y"
            string searchForString = $"@ {screen.Bounds.X},{screen.Bounds.Y}";
            int targetDisplayIndex = displayNames.FindIndex(x => x.Contains(searchForString));
            int targetDisplayValue = int.Parse(displayValues[targetDisplayIndex].ToString());

            return targetDisplayValue;
        }

        private obs_video_info GenerateObsVideoInfoObject(uint baseWidth, uint baseHeight, uint outputWidth, uint outputHeight)
        {
            return new obs_video_info
            {
                adapter = 0,
                base_width = baseWidth,
                base_height = baseHeight,
                output_width = outputWidth,
                output_height = outputHeight,
                fps_num = GetFrameRate(),
                fps_den = Constants.Video.FPS_DEN,
                graphics_module = "libobs-d3d11.dll",
                output_format = video_format.VIDEO_FORMAT_NV12,
                scale_type = obs_scale_type.OBS_SCALE_BICUBIC,
                colorspace = video_colorspace.VIDEO_CS_601,
                range = video_range_type.VIDEO_RANGE_PARTIAL,
                gpu_conversion = true,
            };
        }
    }
}
