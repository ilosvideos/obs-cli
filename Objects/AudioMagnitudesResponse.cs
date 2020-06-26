namespace obs_cli.Objects
{
    public class AudioMagnitudesResponse : StatusResponse
    {
        public float AudioInputLevel { get; set; }
        public float AudioOutputLevel { get; set; }
    }
}
