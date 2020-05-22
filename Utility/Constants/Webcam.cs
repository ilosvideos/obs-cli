namespace obs_cli.Utility
{
    public static partial class Constants
    {
        public static class Webcam
        {
            public static class Settings
            {
                public static string NameValueDelimiter = ":\\";
                public static string WebcamWindowThreadName = "Webcam window";
            }

            public static class ErrorMessages
            {
                public static string EnableWebcamOnlyFailed = "There was an error enabling webcam only mode.";
                public static string OptimalResolutionNotFound = "Exception thrown while searching for optimal webcam resolution.";
                public static string WebcamNotFound = "The selected webcam could not be found.";
            }

            public static class Parameters
            {
                public static string Height = "height";
                public static string Width = "width";
                public static string Left = "left";
                public static string Top = "top";
                public static string WebcamValue = "webcamValue";
                public static string ShouldUseCustomSettings = "shouldUseCustomSettings";
                public static string Fps = "fps";
                public static string Resolution = "resolution";
                public static string VideoFormat = "videoFormat";
            }
        }
    }
}
