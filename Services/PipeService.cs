using NamedPipeWrapper;
using obs_cli.Commands;
using obs_cli.Data;
using obs_cli.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using vidgrid_recorder_data;

namespace obs_cli.Services
{
    public static class PipeService
    {
        public static void Listen()
        {
            Setup();
            while (true) { }
        }

        public static void Teardown()
        {
            Store.Data.Pipe.Main.Stop();
            Store.Data.Pipe.Magnitude.Stop();

            Store.Data.Pipe.Main = null;
            Store.Data.Pipe.Magnitude = null;
        }

        private static void Setup()
        {
            Store.Data.Pipe.Main = SetupServer(Settings.MainPipeName);
            Store.Data.Pipe.Magnitude = SetupServer(Settings.MagnitudePipeName);
        }

        private static NamedPipeServer<Message> SetupServer(string serverName)
        {
            var derivedServerName = Store.Data.App.ParentProcessId.HasValue ? $"{serverName}-{Store.Data.App.ParentProcessId}" : serverName;
            var server = new NamedPipeServer<Message>(derivedServerName);

            server.ClientMessage += (NamedPipeConnection<Message, Message> conn, Message message) => HandleReceivedMessage(conn, message, serverName);
            
            server.Start();

            Console.WriteLine("Pipe server {0} started!", serverName);
            Loggers.CliLogger.Trace($"Pipe server created on channel: {derivedServerName}");

            return server;
        }

        private static void HandleReceivedMessage(NamedPipeConnection<Message, Message> conn, Message message, string serverName)
        {
            List<string> argumentTokens = new List<string>(message.Text.Split(new string[] { "--" }, StringSplitOptions.None));
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
                        Loggers.CliLogger.Fatal(ex);
                        EmitService.EmitException(commandInstance.Name, ex.Message, ex.StackTrace);
                        Program.Terminate();
                    }
                }
            }
        }
    }
}
