using System;
using System.IO;

namespace obs_cli.Helpers
{
    public static class FileWriteService
    {
        private const string LOG_FILE_NAME = "obs_cli_log.txt";

        public static void WriteTimestampedLineToFile(string value, string filePath = LOG_FILE_NAME)
        {
            var lineToWrite = $"{ DateTime.UtcNow.ToString("o") }: { value }";
            WriteLineToFile(lineToWrite, filePath);
        }

        public static void WriteLineToFile(string value, string filePath = LOG_FILE_NAME)
        {
            using (StreamWriter file = new StreamWriter(filePath, true))
            {
                file.WriteLine(value);
            }
        }

        public static void DeletePreviousLogFile(string filePath = LOG_FILE_NAME)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
