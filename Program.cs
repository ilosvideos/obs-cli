﻿using obs_cli.Commands;
using obs_cli.Data;
using obs_cli.Services;
using System;
using System.Collections.Generic;
using System.Linq;

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

            PipeService.Setup();

            while (true)
            {
                string line = Console.ReadLine();

                List<string> argumentTokens = new List<string>(line.Split(new string[] { "--" }, StringSplitOptions.None));
                if (argumentTokens.Count > 0)
                {
                    string command = argumentTokens.FirstOrDefault().Trim();
                    var commandType = AvailableCommandLookup.All.FirstOrDefault(x => x.Key == command);

                    if (!commandType.Equals(default(KeyValuePair<string, Type>)))
                    {
                        IDictionary<string, string> parameters = argumentTokens.Skip(1).Select(x => x.Split('=')).ToDictionary(y => y[0].Trim(), z => z[1].Trim());
                        ICommand commandInstance = (ICommand)Activator.CreateInstance(commandType.Value, parameters);

                        try
                        {
                            commandInstance.Handle();
                        }
                        catch (Exception ex)
                        {
                            // todo: we probably don't want to shutdown on every single exception but let's just do a 
                            // catch all for now
                            PipeService.Teardown();
                            EmitService.EmitException(commandInstance.Name, ex.Message, ex.StackTrace);
                            Environment.Exit(0);
                        }
                    }
                }
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception)e.ExceptionObject;
            EmitService.EmitException("AppDomain", exception.Message, exception.StackTrace);
        }
    }
}
