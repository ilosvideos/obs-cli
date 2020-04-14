﻿using System;
using System.ComponentModel;

namespace obs_cli.Enums
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum value)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
