using obs_cli.Objects;

namespace obs_cli.Data.Modules
{
    public class Audio
    {
        public Source InputSource { get; set; }
        public Item InputItem { get; set; }

        public string CurrentInputId { get; set; }
        public VolMeter InputMeter { get; set; }

        public Source OutputSource { get; set; }
        public Item OutputItem { get; set; }

        public string CurrentOutputId { get; set; }
        public VolMeter OutputMeter { get; set; }
    }
}
