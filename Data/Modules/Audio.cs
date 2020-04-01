using obs_cli.Objects;

namespace obs_cli.Data.Modules
{
    public class Audio
    {
        public Source AudioInputSource { get; set; }
        public Item AudioInputItem { get; set; }

        public string CurrentAudioInputId { get; set; }
        public VolMeter AudioInputMeter { get; set; }

        public Source AudioOutputSource { get; set; }
        public Item AudioOutputItem { get; set; }

        public string CurrentAudioOutputId { get; set; }
        public VolMeter AudioOutputMeter { get; set; }
    }
}
