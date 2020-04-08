using OBS;
using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Helpers;
using obs_cli.Objects;
using obs_cli.Services;
using obs_cli.Utility;
using System;
using System.Collections.Generic;
using System.IO;

namespace obs_cli.Commands.Implementations
{
    public class StartRecording : ICommand
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
        public string VideoOutputFolder { get; set; }

        public string LastVideoName;

        public static string Name
        {
            get
            {
                return "start-recording";
            }
        }

        public StartRecording(IDictionary<string, string> arguments)
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
            this.VideoOutputFolder = arguments["videoOutputFolder"];
        }

        public void Execute()
        {
            FileWriteService.WriteLineToFile("in start recording execute");

            try 
            {
                bool resetVideoInfoStatus = VideoService.ResetVideoInfo(new ResetVideoInfoParameters
                {
                    CropTop = CropTop,
                    CropRight = CropRight,
                    CropLeft = CropLeft,
                    CropBottom = CropBottom,
                    FrameRate = FrameRate,
                    OutputWidth = OutputWidth,
                    OutputHeight = OutputHeight,
                    CanvasWidth = CanvasWidth,
                    CanvasHeight = CanvasHeight,
                    ScreenToRecordHandle = ScreenToRecordHandle
                });

                FileWriteService.WriteLineToFile($"ResetVideoInfo status: {resetVideoInfoStatus}");

                ObsOutputAndEncoders outputAndEncoders = CreateNewObsOutput();
                Store.Data.Obs.OutputAndEncoders = outputAndEncoders;
                Store.Data.Obs.OutputAndEncoders.obsOutput.Start();

                FileWriteService.WriteLineToFile("recording started");
            }
            catch(Exception ex)
            {
                FileWriteService.WriteLineToFile(ex.Message);
            }
        }

        private ObsOutputAndEncoders CreateNewObsOutput()
        {
            ObsOutputAndEncoders outputAndEncoders = new ObsOutputAndEncoders();

            outputAndEncoders.obsVideoEncoder = createVideoEncoder();
            outputAndEncoders.obsAudioEncoder = createAudioEncoder();
            outputAndEncoders.obsOutput = createOutput();

            outputAndEncoders.obsOutput.SetVideoEncoder(outputAndEncoders.obsVideoEncoder);
            outputAndEncoders.obsOutput.SetAudioEncoder(outputAndEncoders.obsAudioEncoder);

            return outputAndEncoders;
        }

        private ObsEncoder createVideoEncoder()
        {
            ObsEncoder obsVideoEncoder = new ObsEncoder(ObsEncoderType.Video, "obs_x264", "simple_h264_stream");
            //obsVideoEncoder.Dispose();
            IntPtr obsVideoPointer = Obs.GetVideo();
            FileWriteService.WriteLineToFile($"using {obsVideoPointer} video pointer");
            obsVideoEncoder.SetVideo(obsVideoPointer);

            ObsData videoEncoderSettings = new ObsData();
            videoEncoderSettings.SetInt("bitrate", Constants.Video.ENCODER_BITRATE);
            videoEncoderSettings.SetString("rate_control", Constants.Video.RATE_CONTROL);
            obsVideoEncoder.Update(videoEncoderSettings);
            videoEncoderSettings.Dispose();

            return obsVideoEncoder;
        }

        private ObsEncoder createAudioEncoder()
        {
            // mf_aac for W8 and later, ffmpeg_aac for W7
            string encoderId = "mf_aac";

            // Windows 7 is Version 6.1. Check if version 6.1 and below. We don't support anything below Windows 7. 
            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT &&
                ((System.Environment.OSVersion.Version.Major == 6 && System.Environment.OSVersion.Version.Minor <= 1) ||
                 System.Environment.OSVersion.Version.Major < 6)
            ) encoderId = "ffmpeg_aac";

            ObsEncoder obsAudioEncoder = new ObsEncoder(ObsEncoderType.Audio, encoderId, "simple_aac");
            obsAudioEncoder.SetAudio(Obs.GetAudio());

            ObsData audioEncoderSettings = new ObsData();
            audioEncoderSettings.SetInt("bitrate", Constants.Audio.ENCODER_BITRATE);
            audioEncoderSettings.SetString("rate_control", Constants.Audio.RATE_CONTROL);
            audioEncoderSettings.SetInt("samplerate", Constants.Audio.SAMPLES_PER_SEC);
            audioEncoderSettings.SetBoolDefault("allow he-aac", true);
            obsAudioEncoder.Update(audioEncoderSettings);
            audioEncoderSettings.Dispose();

            return obsAudioEncoder;
        }

        private ObsOutput createOutput()
        {
            // Output
            string videoDirectory = $"{FolderService.GetPath(KnownFolder.Videos)}\\{VideoOutputFolder}";
            if (Store.Data.Record.RecordedFiles.Count == 0)
            {
                LastVideoName = $"ScreenRecording {DateTime.Now:yyyy-MM-dd HH.mm.ss}";
            }
            string videoFileName = LastVideoName + "_part " + (Store.Data.Record.RecordedFiles.Count + 1) + ".mp4";
            string videoFilePath = $"{videoDirectory}\\{videoFileName}";
            Store.Data.Record.RecordedFiles.Add(new FileInfo(videoFilePath));

            Directory.CreateDirectory(videoDirectory);
            videoFilePath = videoFilePath.Replace("\\", "/"); // OBS uses forward slashes

            ObsOutput obsOutput = new ObsOutput(ObsOutputType.Dummy, "ffmpeg_muxer", "simple_file_output");
            //obsOutput.Dispose();

            ObsData outputSettings = new ObsData();
            outputSettings.SetString("path", videoFilePath);
            outputSettings.SetString("muxer_settings", "movflags=faststart");
            obsOutput.Update(outputSettings);
            outputSettings.Dispose();

            return obsOutput;
        }
    }
}
