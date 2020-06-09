using obs_cli.Objects.Obs;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using static OBS.libobs;

namespace obs_cli.Data.Modules
{
    public class Record
    {
        public Screen ActiveScreen { get; set; }
        public obs_sceneitem_crop AppliedCrop { get; set; }
        public bool IsFullScreen { get; set; }
        public bool IsPausing { get; set; }
        public string LastVideoName { get; set; }
        public ObsOutputAndEncoders OutputAndEncoders { get; set; }
        public List<FileInfo> RecordedFiles { get; set; }
        public string VideoOutputFolder { get; set; }

        public int CanvasHeight { get; set; }
        public int CanvasWidth { get; set; }

        public Record()
        {
            this.RecordedFiles = new List<FileInfo>();
        }
    }
}
