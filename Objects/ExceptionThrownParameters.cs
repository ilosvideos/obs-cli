using System.Collections.Generic;

namespace obs_cli.Objects
{
    public class ExceptionThrownParameters
    {
        public string Message { get; set; }

        public virtual IDictionary<string, string> ToDictionary()
        {
            return new Dictionary<string, string>()
            {
                { "message", Message.ToString() }
            };
        }
    }
}
