﻿using System;
using OBS;

namespace obs_cli.Objects.Obs
{
    public class Filter : ObsSource
    {
        public Filter(string id, string name)
            : base(ObsSourceType.Filter, id, name)
        {

        }

        public Filter(string id, string name, ObsData settings)
            : base(ObsSourceType.Filter, id, name, settings)
        {
        }

        public Filter(IntPtr instance)
            : base(instance)
        {
        }
    }
}
