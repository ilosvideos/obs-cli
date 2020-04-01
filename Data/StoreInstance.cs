using obs_cli.Data.Modules;

namespace obs_cli.Data
{
    public class StoreInstance
    {
        public StoreInstance()
        {
            this.Audio = new Audio();
            this.Display = new Display();
        }

        public Audio Audio { get; set; }
        public Display Display { get; set; }
    }
}
