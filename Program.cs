using CommandLine;
using System;
using System.IO;

namespace obs_cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       if (o.Verbose)
                       {
                           Console.WriteLine($"Verbose output enabled. Current Arguments: -v {o.Verbose}");
                           Console.WriteLine("Quick Start Example! App is in Verbose mode!");
                       }
                       else
                       {
                           Console.WriteLine($"Current Arguments: -v {o.Verbose}");
                           Console.WriteLine("Quick Start Example!");
                       }

                       if (!string.IsNullOrWhiteSpace(o.Write))
                       {
                           var pathToUse = string.IsNullOrWhiteSpace(o.WritePath) ? @"C:\Users\Chad\Documents\test\test.txt" : o.WritePath;
                           using (StreamWriter file = new StreamWriter(pathToUse, true))
                           {
                               file.WriteLine(o.Write);
                           }
                       }
                   });

            Console.ReadLine();
        }
    }
}
