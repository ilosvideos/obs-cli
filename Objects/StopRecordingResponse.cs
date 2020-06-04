namespace obs_cli.Objects
{
    public class StopRecordingResponse : StatusResponse
    {
        public string VideoFilePath { get; set; }
        public string LastVideoName { get; set; }
    }
}
