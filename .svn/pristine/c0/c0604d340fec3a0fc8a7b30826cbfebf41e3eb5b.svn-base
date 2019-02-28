using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoPole.Chameleon3.Foundation;

namespace TwoPole.Chameleon3.Foundation.Gps
{
    /// <summary>
    /// $GPNTR,024404.00,1,17253.242,+5210.449, -16447.587, -49.685,0004*40
    /// </summary>
    public class NtrData
    {
        /// <summary>
        /// 原始数据
        /// </summary>
        public string OriginalSentence { get; private set; }

        /// <summary>
        /// Time of Position Reading
        /// </summary>
        public DateTime UtcTime { get; private set; }

        /// <summary>
        /// 解算状态 0：无效解；1：单点定位解；2：伪距差分；4：固定解；5：浮动解；
        /// </summary>
        public int PositionStatus { get; private set; }

        /// <summary>
        /// 基线距离（米）
        /// </summary>
        public double Distance { get; private set; }

        /// <summary>
        /// 北边的距离（米）
        /// </summary>
        public double Northing { get; private set; }
        /// <summary>
        /// 东边的距离（米）
        /// </summary>
        public double Easting { get; private set; }
        /// <summary>
        /// 海拔高度（米）
        /// </summary>
        public double Vertical { get; private set; }

        /// <summary>
        /// 基站号
        /// </summary>
        public string SatelliteID { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int CheckSum { get; private set; }

        public static NtrData Parse(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return null;

            var endIndex = inputString.IndexOf("*", StringComparison.Ordinal);
            if (endIndex < 0)
                return null;

            string dataString = inputString.Substring(0, endIndex); // strip off the checksum
            var words = dataString.Split(',');
            NtrData data = new NtrData();
            data.OriginalSentence = inputString;
            if (words.Length < 8)
                return data;

            if (words[1].Length == 9)
            {
                int hour = int.Parse(words[1].Substring(0, 2));
                int minute = int.Parse(words[1].Substring(2, 2));
                int second = int.Parse(words[1].Substring(4, 2));
                int millisecond = 0;
                if (words[1].Length >= 9)
                    millisecond = Convert.ToInt32(double.Parse(words[1].Substring(6, 3)) * 1000);
                data.UtcTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, second, millisecond);
            }
            data.PositionStatus = words[2].ToInt32();
            data.Distance = words[3].ToDouble();
            data.Northing = words[4].ToDouble();
            data.Easting = words[5].ToDouble();
            data.Vertical = words[6].ToDouble();
            data.SatelliteID = words[7];
            data.CheckSum = inputString.Substring(endIndex + 1).ToInt32(-1);

            return data;
        }
    }
}
