namespace obs_cli.Utility
{
    public static partial class Constants
    {
        public static class Audio
        {
            public const int SAMPLES_PER_SEC = 44100;
            public const int ENCODER_BITRATE = 160;
            public const string RATE_CONTROL = "VBR";

            public const string NO_DEVICE_ID = "none";
            public const string DEFAULT_DEVICE_ID = "default";

            /* Custom audio delays in nanoseconds */
            public const int NANOSEC_PER_MSEC = 1000000;
            public const int DELAY_OUTPUT = 67 * NANOSEC_PER_MSEC;
            public const int DELAY_INPUT = 137 * NANOSEC_PER_MSEC;
            public const int DELAY_INPUT_NOT_ATTACHED_TO_WEBCAM = 234 * NANOSEC_PER_MSEC;
            public const int DELAY_INPUT_ATTACHED_TO_WEBCAM = 200 * NANOSEC_PER_MSEC;

            public static class Defaults
            {
                public static string AudioInputName = "Primary Sound Capture Device";
                public static string AudioOutputName = "Primary Sound Output Device";
            }

            public static class SettingKeys
            {
                public static string DeviceId = "device_id";
                public static string UseDeviceTiming = "use_device_timing";
                public static string WasapiInputCapture = "wasapi_input_capture";
                public static string WasapiOutputCapture = "wasapi_output_capture";
            }

            public static class Settings
            {
                public static string DefaultDeviceName = "Default";
                public static string AudioDeviceNoneName = "None";
                public static string WasapiInputCaptureName = "Mic";
                public static string WasapiOutputCaptureName = "Desktop Audio";
            }
        }
    }
}
