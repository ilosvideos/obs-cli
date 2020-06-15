namespace obs_cli.Commands
{
    public interface ICommand
    {
        string Name { get; }
        void Execute();
    }
}
