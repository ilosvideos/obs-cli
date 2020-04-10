using System.Collections.Generic;
using System.IO;

namespace obs_cli.Data.Modules
{
    public class Record
    {
        public List<FileInfo> RecordedFiles = new List<FileInfo>();
        public string LastVideoName { get; set; }
    }
}
