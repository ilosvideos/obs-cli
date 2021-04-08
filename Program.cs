using obs_cli.Data;
using obs_cli.Objects;
using obs_cli.Services;
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
            ExceptionService.HandleException(exception, "AppDomain");
        }
    }
}
