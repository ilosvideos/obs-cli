namespace obs_cli.Objects
{
    public class VideoMergeOutput
    {
        public bool IsSuccessful { get; set; }
        public string FileOutputPath { get; set; }
        public string MergeFailureReason { get; set; }

        public VideoMergeOutput()
        {
            IsSuccessful = true;
        }
    }
}
