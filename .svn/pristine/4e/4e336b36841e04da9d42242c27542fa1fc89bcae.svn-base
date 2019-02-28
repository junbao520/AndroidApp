using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace TwoPole.Chameleon3.Infrastructure
{
    public static partial class EnumExtensions
    {

        public static string GetDescription(this Enum value, bool nameInstend = true)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }
            FieldInfo field = type.GetField(name);
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute == null && nameInstend == true)
            {
                return name;
            }
            return attribute == null ? null : attribute.Description;
        }


        private static string GetStringValue(Type enumType, ulong value)
        {
            if ((value & 0x8000000000000000UL) > 0)
            {
                switch (Type.GetTypeCode(enumType))
                {
                    case TypeCode.SByte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                        long longValue = unchecked((long)value);
                        return longValue.ToString(CultureInfo.CurrentCulture);
                }
            }
            return value.ToString(CultureInfo.CurrentCulture);
        }

        public static TEnum ToEnum<TEnum>(this string value)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        public static TEnum ToEnum<TEnum>(this string value, bool ignoreCase)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
        }


        private static bool ParseString(string str, out ulong value)
        {
            char firstChar = str[0];
            if (char.IsDigit(firstChar) || firstChar == '+')
            {
                return ulong.TryParse(str, out value);
            }
            else if (firstChar == '-')
            {
                long valueL;
                if (long.TryParse(str, out valueL))
                {
                    value = unchecked((ulong)valueL);
                    return true;
                }
            }
            value = 0;
            return false;
        }

    }
}
