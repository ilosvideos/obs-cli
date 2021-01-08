using NamedPipeWrapper;
using obs_cli.Data;
using System;
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

        public static bool Teardown()
        {
            Store.Data.Pipe.Main.Stop();
            Store.Data.Pipe.Magnitude.Stop();

            Store.Data.Pipe.Main = null;
            Store.Data.Pipe.Magnitude = null;

            return true;
        }

        private static NamedPipeServer<Message> SetupServer(string serverName)
        {
            var server = new NamedPipeServer<Message>(serverName);
            server.ClientConnected += delegate (NamedPipeConnection<Message, Message> conn)
            {
                Console.WriteLine("Client {0} is now connected to {1}!", conn.Id, serverName);
                conn.PushMessage(new Message { Text = "Welcome from the server!" });
            };

            server.ClientMessage += delegate (NamedPipeConnection<Message, Message> conn, Message message)
            {
                Console.WriteLine("Client {0} says: {1} on {2}", conn.Id, message.Text, serverName);
            };

            server.Start();

            Console.WriteLine("Pipe server {0} started!", serverName);

            return server;
        }
    }
}
