using obs_cli.Data;
using obs_cli.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;

namespace obs_cli.Commands.Implementations
{
    public class StopRecording : ICommand
    {
        public static string Name
        {
            get
            {
                return "stop-recording";
            }
        }

        private Timer outputStopTimer;

        public List<TimeSpan> recordedFilesDurations = new List<TimeSpan>();
        public Stopwatch recordDurationStopWatch = new Stopwatch();

        public StopRecording(IDictionary<string, string> arguments)
        {

        }

        public void Execute()
        {
            Store.Data.Obs.OutputAndEncoders.obsOutput.Stop();

            outputStopTimer = new Timer();
            outputStopTimer.Interval = 50;
            outputStopTimer.Elapsed += new ElapsedEventHandler(StopRecordingWhenOutputInactive);
            outputStopTimer.Enabled = true;
            outputStopTimer.Start();
        }

        private void StopRecordingWhenOutputInactive(object source, ElapsedEventArgs e)
        {
            if (Store.Data.Obs.OutputAndEncoders.obsOutput != null && Store.Data.Obs.OutputAndEncoders.obsOutput.Active)
            {
                return;
            }

            outputStopTimer.Stop();
            outputStopTimer.Dispose();
            outputStopTimer = null;

            Store.Data.Obs.OutputAndEncoders.Dispose();

            recordDurationStopWatch.Reset();
            recordedFilesDurations.Clear();

            var videoMerge = new VideoMerge(Store.Data.Record.RecordedFiles);
            videoMerge.CombineAndWrite();
        }
    }
}
