using obs_cli.Helpers;

namespace obs_cli.Commands.Implementations
{
    public class Initialize : ICommand
    {
        public static string Name
        {
            get
            {
                return "initialize";
            }
        }

        public void Execute()
        {
            FileWriteService.WriteToFile("in initialize execute");
            FileWriteService.WriteToFile("this is after the execute?");
        }
    }
}
