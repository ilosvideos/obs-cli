﻿using OBS;
using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Objects.Obs;
using obs_cli.Utility;
using System;
using System.IO;

namespace obs_cli.Services
{
    public static class ObsService
    {
        /// <summary>
        /// Creates all new Obs output objects.
        /// </summary>
        /// <param name="lastVideoName"></param>
        /// <param name="videoOutputFolder"></param>
        /// <returns></returns>
        public static ObsOutputAndEncoders CreateNewObsOutput(string lastVideoName, string videoOutputFolder)
        {
            ObsOutputAndEncoders outputAndEncoders = new ObsOutputAndEncoders();

            outputAndEncoders.obsVideoEncoder = CreateVideoEncoder();
            outputAndEncoders.obsAudioEncoder = CreateAudioEncoder();
            outputAndEncoders.obsOutput = CreateOutput(lastVideoName, videoOutputFolder);

            outputAndEncoders.obsOutput.SetVideoEncoder(outputAndEncoders.obsVideoEncoder);
            outputAndEncoders.obsOutput.SetAudioEncoder(outputAndEncoders.obsAudioEncoder);

            return outputAndEncoders;
        }

        /// <summary>
        /// Creates a new Obs video encoder with default settings.
        /// </summary>
        /// <returns></returns>
        public static ObsEncoder CreateVideoEncoder()
        {
            ObsEncoder obsVideoEncoder = new ObsEncoder(ObsEncoderType.Video, "obs_x264", "simple_h264_stream");
            IntPtr obsVideoPointer = Obs.GetVideo();
            obsVideoEncoder.SetVideo(obsVideoPointer);

            ObsData videoEncoderSettings = new ObsData();
            videoEncoderSettings.SetInt("bitrate", Constants.Video.ENCODER_BITRATE);
            videoEncoderSettings.SetString("rate_control", Constants.Video.RATE_CONTROL);
            obsVideoEncoder.Update(videoEncoderSettings);
            videoEncoderSettings.Dispose();

            return obsVideoEncoder;
        }

        public static ObsEncoder CreateAudioEncoder()
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

        /// <summary>
        /// Creates the file output.
        /// </summary>
        /// <param name="lastVideoName"></param>
        /// <param name="videoOutputFolder"></param>
        /// <returns></returns>
        public static ObsOutput CreateOutput(string lastVideoName, string videoOutputFolder)
        {
            string videoDirectory = $"{FolderService.GetPath(KnownFolder.Videos)}\\{videoOutputFolder}";
            if (Store.Data.Record.RecordedFiles.Count == 0)
            {
                lastVideoName = $"ScreenRecording {DateTime.Now:yyyy-MM-dd HH.mm.ss}";
            }
            string videoFileName = lastVideoName + "_part " + (Store.Data.Record.RecordedFiles.Count + 1) + ".mp4";
            string videoFilePath = $"{videoDirectory}\\{videoFileName}";
            Store.Data.Record.RecordedFiles.Add(new FileInfo(videoFilePath));

            Directory.CreateDirectory(videoDirectory);
            videoFilePath = videoFilePath.Replace("\\", "/"); // OBS uses forward slashes

            ObsOutput obsOutput = new ObsOutput(ObsOutputType.Dummy, "ffmpeg_muxer", "simple_file_output");

            ObsData outputSettings = new ObsData();
            outputSettings.SetString("path", videoFilePath);
            outputSettings.SetString("muxer_settings", "movflags=faststart");
            obsOutput.Update(outputSettings);
            outputSettings.Dispose();

            return obsOutput;
        }
    }
}
