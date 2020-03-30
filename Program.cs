using obs_cli.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace obs_cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("starting");

            while (true)
            {
                string line = Console.ReadLine();

                List<string> argumentTokens = new List<string>(line.Split(null));
                if (argumentTokens.Count > 0)
                {
                    string command = argumentTokens.FirstOrDefault();
                    var commandType = AvailableCommands.All.GetValueOrDefault(command);

                    if (commandType != null)
                    {
                        IDictionary<string, string> parameters = argumentTokens.Skip(1).Select(x => x.Split('=')).ToDictionary(y => y[0], z => z[1]);
                        ICommand commandInstance = (ICommand)Activator.CreateInstance(commandType, parameters);
                        commandInstance.Execute();
                    }
                }

            }
        }
    }
}
