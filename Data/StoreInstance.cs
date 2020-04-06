using obs_cli.Data.Modules;

namespace obs_cli.Data
{
    public class StoreInstance
    {
        public StoreInstance()
        {
            this.Audio = new Audio();
            this.Display = new Display();
            this.Obs = new Obs();
            this.Record = new Record();
        }

        public Audio Audio { get; set; }
        public Display Display { get; set; }
        public Obs Obs { get; set; }
        public Record Record{ get; set; }
    }
}
