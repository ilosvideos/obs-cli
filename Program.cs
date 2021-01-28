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

            PipeService.Listen();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception)e.ExceptionObject;
            EmitService.EmitException("AppDomain", exception.Message, exception.StackTrace);

            Loggers.CliLogger.Error(exception);
        }
    }
}
