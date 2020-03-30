using OBS;

namespace obs_cli.Objects
{
    public class VolMeter : ObsVolMeter
    {
        public VolMeter() : base(ObsFaderType.OBS_FADER_LOG)
        {
			
        }

		// Levels are a range from 0.0f - 1.0f
		public float Level { get; set; }
	}
}