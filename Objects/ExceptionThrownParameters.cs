using System.Collections.Generic;

namespace obs_cli.Objects
{
    public class ExceptionThrownParameters
    {
        public string CommandName { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        public virtual IDictionary<string, string> ToDictionary()
        {
            return new Dictionary<string, string>()
            {
                { "commandName", CommandName.ToString() },
                { "message", Message.ToString() },
                { "stackTrace", StackTrace.ToString() }
            };
        }
    }
}
