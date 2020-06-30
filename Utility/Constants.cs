namespace obs_cli.Utility
{
	// todo: each one of these classes should realistically get broken out into its own file/class
	partial class Constants
	{
		public static class Video
		{
			public const int FPS = 15;
			public const int FPS_DEN = 1;
			public const int ENCODER_BITRATE = 2500;
			public const string RATE_CONTROL = "VBR";
		}

		public static class RegexPatterns
		{
			public const string Guid =
				@"(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}";
		}

		public static class RegistryKeys
		{
			public static string CLASSES = "Classes";
			public static string DEFAULT_ICON = "DefaultIcon";
			public static string ILOS_CO = "ilosco";
			//public static string ILOS_RECORD = Settings.Default.IlosUrlProtocol;
			public static string INSTALLER_PATH_KEY = "InstallerPath";
			public static string PROXY_HOST = "proxy_host";
			public static string PROXY_PORT = "proxy_port";
			public static string PROXY_SET = "proxy_set";
			public static string RECORDER = "recorder";
			public static string SOFTWARE = "SOFTWARE";
			public static string URL_PROTOCOL = "URL Protocol";
			public static string VIDGRID = "VidGrid";
		}

		// See https://wiki.winehq.org/List_Of_Windows_Messages for more codes.
		internal enum WindowMessage
		{
			WM_DPICHANGED = 0x02E0,

			WM_ENTERSIZEMOVE = 0x0231,
			WM_EXITSIZEMOVE = 0x0232,

			WM_MOVE = 0x0003,
			WM_SIZE = 0x0005,

			WM_ACTIVATE = 0x0006,

			WM_DWMCOLORIZATIONCOLORCHANGED = 0x0320,

			WM_NCLBUTTONDOWN = 0xA1,
			HTCAPTION = 0x2,

			// mouse events
			WM_LBUTTONDOWN = 0x0201,
			WM_LBUTTONUP = 0x0202,
			WM_MOUSEMOVE = 0x0200,
			WM_MOUSEWHEEL = 0x020A,
			WM_RBUTTONDOWN = 0x0204,
			WM_RBUTTONUP = 0x0205
		}
	}
}
