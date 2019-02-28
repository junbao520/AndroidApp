using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoPole.Chameleon3.Foundation.Gps
{
    public class UtmData
    {
        /// <summary>
        /// 原始数据
        /// </summary>
        public string OriginalSentence { get; private set; }
        public string FormatedSentence { get; private set; }
        /// <summary>
        /// NovAtel状态
        /// </summary>
        public NAFixStatus FixStatus { get; private set; }
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
        public double Elevation { get; private set; }
        /// <summary>
        /// 跟踪的卫星颗数
        /// </summary>
        public int SatellitesInTrack { get; private set; }
        /// <summary>
        /// 参与解算的卫星颗数
        /// </summary>
        public int SatellitesInSolution { get; private set; }

        public static UtmData Parse(string inputString)
        {
            if (string.IsNullOrEmpty(inputString) || !inputString.StartsWith("#"))
                return null;

            var startIndex = inputString.IndexOf(";", StringComparison.Ordinal);
            if (startIndex < 0)
                return null;
            var endIndex = inputString.IndexOf("*", StringComparison.Ordinal);
            if (endIndex < 0)
                return null;

            var sentence = inputString.Substring(startIndex, endIndex - startIndex);
            var words = sentence.Split(',');

            UtmData data = new UtmData();
            data.OriginalSentence = inputString;
            data.FormatedSentence = sentence;
            //NovAtel Status
            var fixStatus = NAFixStatus.None;
            if (words.Length >= 2 && words[1].Length != 0)
                Enum.TryParse(words[1], true, out fixStatus);

            data.FixStatus = fixStatus;
            data.Northing = ParseWord(words, 4, double.Parse, 0);
            data.Easting = ParseWord(words, 5, double.Parse, 0);
            data.Elevation = ParseWord(words, 6, double.Parse, 0);
            data.SatellitesInTrack = ParseWord(words, 15, int.Parse, 0);
            data.SatellitesInSolution = ParseWord(words, 18, int.Parse, 0);

            return data;
        }
        private static T ParseWord<T>(string[] words, int index, Func<string, T> converter, T defaultValue)
        {
            if (words.Length > index && words[index].Length != 0)
                return converter(words[index]);
            return defaultValue;
        }
    }
}
