using obs_cli.Data;
using System.Timers;

namespace obs_cli.Commands.Abstract
{
    public abstract class BaseOutputStop : BaseCommand
    {
        private Timer OutputStopTimer { get; set; }

        public BaseOutputStop()
        {
            OutputStopTimer = new Timer();
        }

        protected abstract void OutputStopped();

        public void StopOutput()
        {
            Store.Data.Record.OutputAndEncoders.obsOutput.Stop();

            OutputStopTimer.Interval = 50;
            OutputStopTimer.Elapsed += new ElapsedEventHandler(DisposeEncodersAndCallback);
            OutputStopTimer.Enabled = true;
            OutputStopTimer.Start();
        }

        private void DisposeEncodersAndCallback(object source, ElapsedEventArgs e)
        {
            if (Store.Data.Record.OutputAndEncoders.obsOutput != null && Store.Data.Record.OutputAndEncoders.obsOutput.Active)
            {
                return;
            }

            OutputStopTimer.Stop();
            OutputStopTimer.Dispose();
            OutputStopTimer = null;

            this.OutputStopped();
        }
    }
}
