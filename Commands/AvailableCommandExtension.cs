using System.ComponentModel;

namespace obs_cli.Commands
{
    public static class AvailableCommandExtension
    {
        public static string GetDescription(this AvailableCommand value)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
