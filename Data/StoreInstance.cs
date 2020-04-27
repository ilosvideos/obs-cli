﻿using obs_cli.Data.Modules;

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
            this.Webcam = new Webcam();
        }

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
