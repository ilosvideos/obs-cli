using obs_cli.Data.Modules;
using System.Windows;

namespace obs_cli.Data
{
    public class StoreInstance
    {
        public StoreInstance()
        {
            this.App = new App();
            this.Audio = new Audio();
            this.Display = new Display();
            this.Obs = new Obs();
            this.Record = new Record();
            this.Webcam = new Webcam();
        }

        public App App { get; set; }
        public Audio Audio { get; set; }
        public Display Display { get; set; }
        public Obs Obs { get; set; }
        public Record Record { get; set; }
        public Webcam Webcam { get; set; }

        /// <summary>
        /// Resets the Record store.
        /// </summary>
        public void ResetRecordModule()
        {
            this.Record = new Record();
        }
    }
}
