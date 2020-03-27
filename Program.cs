using obs_cli.Commands;
using obs_cli.Helpers;
using System;
using System.Collections.Generic;

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
                var commandType = AvailableCommands.All.GetValueOrDefault(command);

                if (commandType != null)
                {
                    ICommand commandInstance = (ICommand)Activator.CreateInstance(commandType);
                    commandInstance.Execute();
                }
            }
        }
    }
}
