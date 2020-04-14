using System.Collections.Generic;

namespace obs_cli.Objects
{
    public class AudioMagnitudeParameters
    {
        public float Magnitude { get; set; }

        public virtual IDictionary<string, string> ToDictionary()
        {
            return new Dictionary<string, string>()
            {
                { "magnitude", Magnitude.ToString() }
            };
        }
    }
}
