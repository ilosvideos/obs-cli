namespace obs_cli.Commands
{
    public interface ICommand
    {
        static string Name { get; }
        void Execute();
    }
}
