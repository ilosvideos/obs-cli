using obs_cli.Data.Modules;

namespace obs_cli.Data
{
    public class StoreInstance
    {
        public StoreInstance()
        {
            App = new App();
            Audio = new Audio();
            Display = new Display();
            Obs = new Obs();
            Record = new Record();
            Webcam = new Webcam();
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
            var previousActiveScreen = Record.ActiveScreen;
            Record = new Record
            {
                ActiveScreen = previousActiveScreen
            };
        }
    }
}
