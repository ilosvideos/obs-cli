using System;
using System.IO;

namespace obs_cli.Helpers
{
    public static class FileWriteService
    {
        public static void WriteTimestampedLineToFile(string value, string filePath = "obs_cli_log.txt")
        {
            var lineToWrite = $"{ DateTime.UtcNow.ToString("o") }: { value }";
            WriteLineToFile(lineToWrite, filePath);
        }

        public static void WriteLineToFile(string value, string filePath = "obs_cli_log.txt")
        {
            using (StreamWriter file = new StreamWriter(filePath, true))
            {
                file.WriteLine(value);
            }
        }
    }
}
