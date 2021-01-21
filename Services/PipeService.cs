using NamedPipeWrapper;
using obs_cli.Commands;
using obs_cli.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using vidgrid_recorder_data;

namespace obs_cli.Services
{
    public static class PipeService
    {
        public static void Setup()
        {
            Store.Data.Pipe.Main = SetupServer(Settings.MainPipeName);
            Store.Data.Pipe.Magnitude = SetupServer(Settings.MagnitudePipeName);
        }

        public static void Teardown()
        {
            Store.Data.Pipe.Main.Stop();
            Store.Data.Pipe.Magnitude.Stop();

            Store.Data.Pipe.Main = null;
            Store.Data.Pipe.Magnitude = null;
        }

        private static NamedPipeServer<Message> SetupServer(string serverName)
        {
            var server = new NamedPipeServer<Message>(serverName);

            server.ClientConnected += (NamedPipeConnection<Message, Message> conn) => HandleNewConnection(conn, serverName);
            server.ClientMessage += (NamedPipeConnection<Message, Message> conn, Message message) => HandleReceivedMessage(conn, message, serverName);
            
            server.Start();

            Console.WriteLine("Pipe server {0} started!", serverName);

            return server;
        }

        private static void HandleNewConnection(NamedPipeConnection<Message, Message> conn, string serverName)
        {
            Console.WriteLine("Client {0} is now connected to {1}!", conn.Id, serverName);
            conn.PushMessage(new Message { Text = $"Welcome from {serverName} server!" });
        }

        private static void HandleReceivedMessage(NamedPipeConnection<Message, Message> conn, Message message, string serverName)
        {
            Console.WriteLine("Client {0} says: {1} on {2}", conn.Id, message.Text, serverName);
            Debug.WriteLine($"Client {conn.Id} says: {message.Text} on {serverName}");

            var line = message.Text;

            // message text is going to be the command
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
                        Teardown();
                        EmitService.EmitException(commandInstance.Name, ex.Message, ex.StackTrace);
                        Environment.Exit(0);
                    }
                }
            }
        }
    }
}
