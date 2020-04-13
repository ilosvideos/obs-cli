using obs_cli.Data;
using System.IO;

namespace obs_cli.Services
{
    // todo: should this class and ObsVideoService be merged together into a single VideoService?
    public static class VideoService
    {
        /// <summary>
        /// Cancels the current recording.
        /// </summary>
        public static void CancelRecording()
        {
            Store.Data.Record.OutputAndEncoders.Dispose();

            foreach (FileInfo file in Store.Data.Record.RecordedFiles)
            {
                file.Delete();
            }

            Store.Data.ResetRecordModule();
        }
    }
}
