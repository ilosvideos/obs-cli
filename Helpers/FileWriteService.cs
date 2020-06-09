using System;
using System.IO;

namespace obs_cli.Helpers
{
    public static class FileWriteService
    {
        public static void WriteTimestampedLineToFile(string value, string filePath = "test.txt")
        {
            var lineToWrite = $"{ DateTime.UtcNow.ToString("o") }: { value }";
            WriteLineToFile(lineToWrite);
        }

        public static void WriteLineToFile(string value, string filePath = "test.txt")
        {
            using (StreamWriter file = new StreamWriter(filePath, true))
            {
                file.WriteLine(value);
            }
        }
    }
}
