namespace obs_cli.Objects
{
    public class InitializeResponse : StatusResponse
    {
        public string SetAudioInputDevice { get; set; }
        public string SetAudioOutputDevice { get; set; }
    }
}
