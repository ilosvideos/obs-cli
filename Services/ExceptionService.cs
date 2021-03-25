using obs_cli.Exceptions;
using obs_cli.Objects;
using obs_cli.Utility;
using System;

namespace obs_cli.Services
{
    public static class ExceptionService
    {
        public static void HandleException(Exception exception, string commandName)
        {
            Loggers.CliLogger.Fatal(exception);
            var exceptionMessage = exception is IObsException ? exception.Message : Constants.Exception.MESSAGE;
            EmitService.EmitException(commandName, exceptionMessage, exception.StackTrace);
        }
    }
}
