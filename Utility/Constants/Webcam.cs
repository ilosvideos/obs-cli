namespace obs_cli.Utility
{
    public static partial class Constants
    {
        public static class Webcam
        {
            public static string NameValueDelimiter = ":\\";

            public static class ErrorMessages
            {
                public static string OptimalResolutionNotFound = "Exception thrown while searching for optimal webcam resolution.";
                public static string WebcamNotFound = "The selected webcam could not be found.";
            }
        }
    }
}
