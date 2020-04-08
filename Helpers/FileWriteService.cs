using System.IO;

namespace obs_cli.Helpers
{
    public static class FileWriteService
    {
        public static void WriteLineToFile(string value, string filePath = "test.txt")
        {
            using (StreamWriter file = new StreamWriter(filePath, true))
            {
                file.WriteLine(value);
            }
        }
    }
}
