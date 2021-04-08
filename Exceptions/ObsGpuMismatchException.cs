using System;

namespace obs_cli.Exceptions
{
    [Serializable]
    public class ObsGpuMismatchException : Exception, IObsException 
    {
        private const string MESSAGE = "The screen you have requested cannot be recorded. Please switch launch graphics card and try again.";

        public ObsGpuMismatchException(Exception innerException, string message = MESSAGE)
            :base(message, innerException)
        { }
    }
}
