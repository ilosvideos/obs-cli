using CommandLine;

namespace obs_cli
{
    public class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option("write", Required = false, HelpText = "Write the given value to a file.")]
        public string Write { get; set; }

        [Option("path", Required = false, HelpText = "Write the given value to a path.")]
        public string WritePath { get; set; }
    }
}
