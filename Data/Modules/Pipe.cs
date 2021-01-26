using NamedPipeWrapper;
using vidgrid_recorder_data;

namespace obs_cli.Data.Modules
{
    public class Pipe
    {
        public NamedPipeServer<Message> Main { get; set; }
        public NamedPipeServer<Message> Magnitude { get; set; }

        public void Setup()
        {

        }
    }
}
