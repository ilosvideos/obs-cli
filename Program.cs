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
                string value = Console.ReadLine();
                if (value == "quit")
                {
                    Environment.Exit(0);
                }
                else
                {
                    // it's not quit. make sure it's a valid command
                }
            }
        }
    }
}
