using obs_cli.Data;
using obs_cli.Exceptions;
using obs_cli.Objects;
using obs_cli.Services;
using obs_cli.Utility;
using System;

namespace obs_cli
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Console.WriteLine("starting");
            Store.Data = new StoreInstance();

            if (args.Length > 0)
            {
                int processId = -1;
                if(int.TryParse(args[0], out processId))
                {
                    Store.Data.App.ParentProcessId = processId;
                }
                else
                {
                    Loggers.CliLogger.Error("No parent process ID argument detected.");
                }

            }

            ParentHeartbeatService.Monitor();
            PipeService.Listen();
        }

        public static void Terminate()
        {
            MagnitudeService.Teardown();
            PipeService.Teardown();
            ParentHeartbeatService.Teardown();
            Environment.Exit(0);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception)e.ExceptionObject;
            Loggers.CliLogger.Fatal(exception);

            // todo:
            // before we send exception.Message, we should check to see if it's in our dictionary of messages
            // but what if the exception is the same type/message for multiple scenarios?

            //var exceptionMessage = exception is IObsException ? exception.Message : Constants.Exception.MESSAGE;

            EmitService.EmitException("AppDomain", "test", exception.StackTrace);
        }
    }
}
