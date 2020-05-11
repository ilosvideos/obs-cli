using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace obs_cli.Objects
{
    public class GenerateObsVideoInfoParameters
    {
        public uint BaseWidth { get; set; }
        public uint BaseHeight { get; set; }
        public uint OutputWidth { get; set; }
        public uint OutputHeight { get; set; }
        public uint FrameRate { get; set; }
    }
}
