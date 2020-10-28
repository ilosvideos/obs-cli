using NLog;

namespace obs_cli.Objects
{
    public static class CliLogger
    {
        public static Logger CommandLogger => LogManager.GetLogger("CommandLogger");
        public static Logger OBSLogger => LogManager.GetLogger("OBSLogger");
    }
}
