using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace TwoPole.Chameleon3.Infrastructure
{
    /// <summary>
    /// Contains various extension methods for the string type.
    /// </summary>
    public static class StringExtensions
    {
        #region Convert
        public static sbyte? ToSByte(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return new sbyte?();
            sbyte ret;
            if (!sbyte.TryParse(value, out ret))
                return new sbyte?();
            return ret;
        }

        public static sbyte ToSByte(this string value, sbyte defalutValue)
        {
            var ret = value.ToSByte();
            return ret.HasValue ? ret.Value : defalutValue;
        }

        public static byte? ToByte(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return new byte?();
            byte ret;
            if (!byte.TryParse(value, out ret))
                return new byte?();
            return ret;
        }

        public static byte ToByte(this string value, byte defalutValue)
        {
            var ret = value.ToByte();
            return ret.HasValue ? ret.Value : defalutValue;
        }

        public static ushort? ToUInt16(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return new ushort?();
            ushort ret;
            if (!ushort.TryParse(value, out ret))
                return new ushort?();
            return ret;
        }

        public static ushort ToUInt16(this string value, ushort defalutValue)
        {
            var ret = value.ToUInt16();
            return ret.HasValue ? ret.Value : defalutValue;
        }

        public static short? ToInt16(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return new short?();
            short ret;
            if (!short.TryParse(value, out ret))
                return new short?();
            return ret;
        }

        public static short ToInt16(this string value, short defalutValue)
        {
            var ret = value.ToInt16();
            return ret.HasValue ? ret.Value : defalutValue;
        }

        public static uint? ToUInt32(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return new uint?();
            uint ret;
            if (!uint.TryParse(value, out ret))
                return new uint?();
            return ret;
        }

        public static uint ToUInt32(this string value, uint defalutValue)
        {
            var ret = value.ToUInt32();
            return ret.HasValue ? ret.Value : defalutValue;
        }

        public static int? ToInt32(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return new int?();
            int ret;
            if (!int.TryParse(value, out ret))
                return new int?();
            return ret;
        }

        public static int ToInt32(this string value, int defalutValue)
        {
            var ret = value.ToInt32();
            return ret.HasValue ? ret.Value : defalutValue;
        }

        public static ulong? ToUInt64(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return new ulong?();
            ulong ret;
            if (!ulong.TryParse(value, out ret))
                return new ulong?();
            return ret;
        }

        public static ulong ToUInt64(this string value, ulong defalutValue)
        {
            var ret = value.ToUInt64();
            return ret.HasValue ? ret.Value : defalutValue;
        }

        public static long? ToInt64(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return new long?();
            long ret;
            if (!long.TryParse(value, out ret))
                return new long?();
            return ret;
        }

        public static long ToInt64(this string value, long defalutValue)
        {
            var ret = value.ToInt64();
            return ret.HasValue ? ret.Value : defalutValue;
        }

        public static float? ToSingle(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return new float?();
            float ret;
            if (!float.TryParse(value, out ret))
                return new float?();
            return ret;
        }

        public static float ToSingle(this string value, float defalutValue)
        {
            var ret = value.ToSingle();
            return ret.HasValue ? ret.Value : defalutValue;
        }

        public static double? ToDouble(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return new double?();
            double ret;
            if (!double.TryParse(value, out ret))
                return new double?();
            return ret;
        }

        public static double ToDouble(this string value, double defalutValue)
        {
            var ret = value.ToDouble();
            return ret.HasValue ? ret.Value : defalutValue;
        }

        public static decimal? ToDecimal(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return new decimal?();
            decimal ret;
            if (!decimal.TryParse(value, out ret))
                return new decimal?();
            return ret;
        }

        public static decimal ToDecimal(this string value, decimal defalutValue)
        {
            var ret = value.ToDecimal();
            return ret.HasValue ? ret.Value : defalutValue;
        }

        public static DateTime? ToDateTime(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return new DateTime?();
            DateTime ret;
            if (!DateTime.TryParse(value, out ret))
                return new DateTime?();
            return ret;
        }

        public static DateTime ToDateTime(this string value, DateTime defalutValue)
        {
            var ret = value.ToDateTime();
            return ret.HasValue ? ret.Value : defalutValue;
        }

        public static Guid? ToGuid(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return new Guid?();
            Guid ret;
            if (!Guid.TryParse(value, out ret))
                return new Guid?();
            return ret;
        }

        public static Guid ToGuid(this string value, Guid defalutValue)
        {
            var ret = value.ToGuid();
            return ret.HasValue ? ret.Value : defalutValue;
        }

        public static bool? ToBoolean(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return new bool?();
            bool ret;
            if (!bool.TryParse(value, out ret))
                return new bool?();
            return ret;
        }

        public static bool ToBoolean(this string value, bool defalutValue)
        {
            var ret = value.ToBoolean();
            return ret.HasValue ? ret.Value : defalutValue;
        }

        public static T ToEnum<T>(this string value, T defaultValue) where T : struct
        {
            T ret;
            if (Enum.TryParse<T>(value, out ret))
                return ret;
            return defaultValue;
        }

        #endregion

        /// <summary>
        /// Formats the string with the given parameters using an invariant <see cref="CultureInfo"/>
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }

        public static string RemoveDuplicateWhitespace(this string input)
        {
            return Regex.Replace(input, @"\s", " ", RegexOptions.Compiled);
        }

        public static string RemoveWhitespace(this string input)
        {
            return Regex.Replace(input, @"\s", "", RegexOptions.Compiled);
        }

        public static bool IsIn(this string input, params string[] parameters)
        {
            return input.IsIn(StringComparison.Ordinal, parameters);
        }

        public static bool IsIn(this string input, StringComparison comparison, params string[] parameters)
        {
            if (parameters == null)
                return false;

            return parameters.Any(d => string.Equals(input, d, comparison));
        }

        public static string ToFileSize(this int size)
        {
            var scale = 1024;
            var kb = 1 * scale;
            var mb = kb * scale;
            var gb = mb * scale;
            var tb = gb * scale;

            if (size < kb)
                return size + " Bytes";
            else if (size < mb)
                return ((Double)size / kb).ToString("0.## KB");
            else if (size < gb)
                return ((Double)size / mb).ToString("0.## MB");
            else if (size < tb)
                return ((Double)size / gb).ToString("0.## GB");
            else
                return ((Double)size / tb).ToString("0.## TB");
        }
    }
}
