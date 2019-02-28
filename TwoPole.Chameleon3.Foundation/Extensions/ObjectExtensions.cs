using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
namespace TwoPole.Chameleon3.Foundation
{
    public static class ObjectExtensions
    {
        public static double GetDoubleValue(this NameValueCollection nameValues, string name, double defaultValue)
        {
            string str = nameValues[name];
            if (string.IsNullOrWhiteSpace(str))
                return defaultValue;
            else
                return str.ToDouble(defaultValue);
        }

        public static bool ToBoolean(this int input)
        {
            return input > 0;
        }

        public static string ToError(this Exception exp)
        {
            var current = exp;
            StringBuilder sb = new StringBuilder();
            while (current != null)
            {
                sb.AppendLine(current.ToString());
                current = current.InnerException;
            }
            return sb.ToString();
        }


        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this object instance, string propertyName)
        {
            if(instance == null)
                throw new ArgumentNullException("instance");

            if(string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException("propertyName");

            var property = instance.GetType().GetProperty(propertyName);
            var v = property.GetValue(instance);
            return (T) v;
        }

        //public static NameValueCollection ToNameValueCollection(this NameValueConfigurationCollection values)
        //{
        //    var newValues = new NameValueCollection();
        //    if (values != null)
        //    {
        //        foreach (NameValueConfigurationElement item in values)
        //        {
        //            newValues.Add(item.Name, item.Value);
        //        }
        //    }

        //    return newValues;
        //}

        //public static NameValueCollection Merge(this NameValueConfigurationCollection target1, NameValueConfigurationCollection target2)
        //{
        //    var newValues = new NameValueCollection();
        //    if (target1 != null)
        //    {
        //        foreach (NameValueConfigurationElement item in target1)
        //        {
        //            newValues[item.Name] = item.Value;
        //        }
        //    }
        //    if (target2 != null)
        //    {
        //        foreach (NameValueConfigurationElement item in target2)
        //        {
        //            newValues[item.Name] = item.Value;
        //        }
        //    }

        //    return newValues;
        //}

        public static double ToDouble(this string input, double defaultValue = 0)
        {
            double v;
            if (double.TryParse(input, out v))
                return v;
            return defaultValue;
        }

    }
}
