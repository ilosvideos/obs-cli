using obs_cli.Data;
using obs_cli.Services;
using System;

namespace obs_cli
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("starting");
            Store.Data = new StoreInstance();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            PipeService.Listen();
        }

        // todo: we might want to write a poller that checks the existence of VG recorder app. if it's not present
        // then shut CLI down
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception)e.ExceptionObject;
            EmitService.EmitException("AppDomain", exception.Message, exception.StackTrace);
        }
    }
}
