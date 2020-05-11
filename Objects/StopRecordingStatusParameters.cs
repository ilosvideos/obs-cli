using System.Collections.Generic;

namespace obs_cli.Objects
{
    public class StopRecordingStatusParameters
    {
        public bool IsSuccessful { get; set; }
        public string VideoFilePath { get; set; }
        public string LastVideoName { get; set; }

        public virtual IDictionary<string, string> ToDictionary()
        {
            return new Dictionary<string, string>()
            {
                { "isSuccessful", IsSuccessful.ToString() },
                { "videoFilePath", VideoFilePath.ToString() },
                { "lastVideoName", LastVideoName.ToString() }
            };
        }
    }
}
