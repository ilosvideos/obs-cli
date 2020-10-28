using NLog;

namespace obs_cli.Objects
{
    public static class Loggers
    {
        public static Logger CommandLogger => NLog.LogManager.GetLogger("CommandLogger");
        public static Logger OBSLogger => NLog.LogManager.GetLogger("OBSLogger");
        public static Logger CliLogger => NLog.LogManager.GetLogger("CliLogger");
    }
}
