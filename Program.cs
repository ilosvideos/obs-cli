using obs_cli.Commands;
using obs_cli.Data;
using obs_cli.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace obs_cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("starting");
            Store.Data = new StoreInstance();

            while (true)
            {
                string line = Console.ReadLine();

                Console.WriteLine($"line: {line}");
                FileWriteService.WriteLineToFile($"line: {line}");

                List<string> argumentTokens = new List<string>(line.Split(null));
                if (argumentTokens.Count > 0)
                {
                    string command = argumentTokens.FirstOrDefault();
                    var commandType = AvailableCommands.All.FirstOrDefault(x => x.Key == command);

                    if (!commandType.Equals(default(KeyValuePair<string, Type>)))
                    {
                        IDictionary<string, string> parameters = argumentTokens.Skip(1).Select(x => x.Split('=')).ToDictionary(y => y[0], z => z[1]);
                        ICommand commandInstance = (ICommand)Activator.CreateInstance(commandType.Value, parameters);
                        Console.WriteLine($"executing {commandType.Key}");
                        commandInstance.Execute();
                    }
                }

            }
        }
    }
}
