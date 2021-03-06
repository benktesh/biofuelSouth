﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace BiofuelSouth.Helpers
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this System.Enum enumValue)
        {
            var val = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault();

	        if (val != null)
	        {
		        var attrs = val.GetCustomAttributes<DisplayAttribute>().FirstOrDefault();
				if ( attrs == null )
				{
					return enumValue.ToString();
				}

				return attrs
					.GetName();
			}

	        return enumValue.ToString(); //could not find the enum value

        }

        public static string StringValue(this System.Enum value)
        {
            if (value == null)
            {
                return String.Empty;
            }

            var fi = value.GetType().GetField(value.ToString());
            var attrs = fi.GetCustomAttributes(typeof (DisplayAttribute), false) as DisplayAttribute[];

            if (attrs != null && attrs.Length > 0)
            {
                return attrs[0].Name;
            }

            return value.ToString();
        }
    }
}