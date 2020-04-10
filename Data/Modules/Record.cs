using obs_cli.Objects.Obs;
using System.Collections.Generic;
using System.IO;
using static OBS.libobs;

namespace obs_cli.Data.Modules
{
    public class Record
    {
        public obs_sceneitem_crop AppliedCrop { get; set; }
        public string LastVideoName { get; set; }
        public ObsOutputAndEncoders OutputAndEncoders { get; set; }

        public List<FileInfo> RecordedFiles = new List<FileInfo>();

        public string VideoOutputFolder { get; set; }
    }
}
