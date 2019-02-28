using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using TwoPole.Chameleon3.Domain;

namespace TwoPole.Chameleon3.Infrastructure
{
    public static class ObjectExtensions
    {

        private static readonly JsonSerializerSettings JsonSettings;
        static ObjectExtensions()
        {
            JsonSettings = new JsonSerializerSettings();
           
        }
        #region FromJson - Json.NET

        public static T FromJson<T>(this string json)
        {
            try
            {
                if (!string.IsNullOrEmpty(json))
                    return JsonConvert.DeserializeObject<T>(json, JsonSettings);
            }
            catch
            { }
            return default(T);
        }
        #endregion

        public static string ToJson(this Object entity)
        {
            if (entity == null)
                return null;
            return JsonConvert.SerializeObject(entity, Formatting.None, JsonSettings);
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

        //public static IEnumerable<System.Drawing.Point> ToPoints(this IEnumerable<EndPoint> list)
        //{
        //    return list.Select(p => new System.Drawing.Point(p.X, p.Y));
        //}

        //public static System.Drawing.Point ToPoint(this IntPoint p)
        //{
        //    return new System.Drawing.Point(p.X, p.Y);
        //}

        public static string ToGearDisplayText(this CarSensorInfo carDevice)
        {
            if (carDevice == null)
                return string.Empty;
            return carDevice.Gear.ToDisplayText();
        }

        public static string ToDisplayGearText(this CarSensorInfo carDevice)
        {
            if (carDevice == null)
                return string.Empty;
            return carDevice.Gear.ToDisplayGearText();
        }

        public static string ToDisplayText(this Gear gear)
        {
            switch (gear)
            {
                case Gear.Neutral:
                    return "空档";
                case Gear.Reverse:
                    return "倒档";
                default:
                    return (int)gear + "档";
            }
        }

        public static string ToDisplayGearText(this Gear gear)
        {
            switch (gear)
            {
                case Gear.Neutral:
                    return "0";
                case Gear.One:
                    return "1";
                case Gear.Two:
                    return "2";
                case Gear.Three:
                    return "3";
                case Gear.Four:
                    return "4";
                case Gear.Five:
                    return "5";
                default:
                    return (int)gear + "档";
            }
        }
    }
}
