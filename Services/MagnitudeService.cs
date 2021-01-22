using obs_cli.Data;
using obs_cli.Objects;
using System;
using System.Timers;
using System.Web.Script.Serialization;
using vidgrid_recorder_data;

namespace obs_cli.Services
{
    public static class MagnitudeService
    {
        private const int MAGNITUDE_INTERVAL = 33;

        private static Timer _magnitudeTimer { get; set; }

        public static void Setup()
        {
            _magnitudeTimer = new Timer();
            _magnitudeTimer.Interval = MAGNITUDE_INTERVAL;
            _magnitudeTimer.Elapsed += SendMagnitudes;
            _magnitudeTimer.Start();
        }

        // As of OBS 21.0.1, audo meters have been reworked. We now need to calculate and draw ballistics ourselves. 
        // Relevant commit: https://github.com/obsproject/obs-studio/commit/50ce2284557b888f230a1730fa580e82a6a133dc#diff-505cedf4005a973efa8df1e299be4199
        // This is probably an over-simplified calculation.
        // For practical purposes, we are treating -60 as 0 and -9 as 1.
        public static void InputVolumeCallback(IntPtr data, float[] magnitude, float[] peak, float[] input_peak)
        {
            Store.Data.Audio.InputMeter.Level = magnitude[0];
        }

        public static void OutputVolumeCallback(IntPtr data, float[] magnitude, float[] peak, float[] input_peak)
        {
            Store.Data.Audio.OutputMeter.Level = magnitude[0];
        }

        public static void Teardown()
        {
            Store.Data.Audio.InputMeter.RemoveCallback();
            Store.Data.Audio.OutputMeter.RemoveCallback();

            _magnitudeTimer.Stop();
            _magnitudeTimer.Dispose();
            _magnitudeTimer = null;
        }

        private static void SendMagnitudes(object sender, ElapsedEventArgs e)
        {
            var magnitudes = new AudioMagnitudesResponse
            {
                IsSuccessful = true,
                AudioInputLevel = Store.Data.Audio.InputMeter.Level,
                AudioOutputLevel = Store.Data.Audio.OutputMeter.Level
            };

            Store.Data.Pipe.Magnitude.PushMessage(new Message { Text = new JavaScriptSerializer().Serialize(magnitudes) });
        }
    }
}
