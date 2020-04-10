namespace obs_cli.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public abstract string Name { get; }

        public abstract void Execute();
    }
}
