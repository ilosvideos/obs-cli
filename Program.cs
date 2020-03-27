using obs_cli.Helpers;
using System;

namespace obs_cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("starting");

            while (true)
            {
                string command = Console.ReadLine();
                if (command == "quit")
                {
                    FileWriteService.WriteToFile("quitting");
                    Environment.Exit(0);
                }
                else
                {
                    // it's not quit. make sure it's a valid command
                    FileWriteService.WriteToFile($"received command {command}");
                }
            }
        }
    }
}
