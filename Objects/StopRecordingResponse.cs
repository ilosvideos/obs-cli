using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace obs_cli.Objects
{
    public class StopRecordingResponse : StatusResponse
    {
        public string VideoFilePath { get; set; }
        public string LastVideoName { get; set; }
    }
}
