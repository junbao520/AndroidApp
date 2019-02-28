using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoPole.Chameleon3.Foundation.Gps
{
    public class HgaData
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
        /// 基线长
        /// </summary>
        public double Baseline { get; private set; }
        /// <summary>
        /// 航向角
        /// </summary>
        public double BearingDegrees { get; private set; }
        /// <summary>
        /// 俯仰角 
        /// </summary>
        public double ElevationDegrees { get; private set; }
        /// <summary>
        /// 跟踪的卫星颗数
        /// </summary>
        public int SatellitesInTrack { get; private set; }
        /// <summary>
        /// 参与解算的卫星颗数
        /// </summary>
        public int SatellitesInSolution { get; private set; }

        public static HgaData Parse(string inputString)
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

            HgaData data = new HgaData();
            data.OriginalSentence = inputString;
            data.FormatedSentence = sentence;
            //NovAtel Status
            data.FixStatus = NAFixStatus.None;
            if (words.Length >= 2 && words[1].Length != 0)
            {
                try
                {
                    data.FixStatus = (NAFixStatus)Enum.Parse(typeof(NAFixStatus), words[1], true);
                }
                catch (Exception ex)
                {
                    string str = ex.Message;
                }
            }

            data.Baseline = ParseWord(words, 2, double.Parse, 0);
            // The bearing is the seventh word
            data.BearingDegrees = ParseWord(words, 3, double.Parse, 0);
            // The elevation is the seventh word
            data.ElevationDegrees = ParseWord(words, 4, double.Parse, 0);
            data.SatellitesInTrack = ParseWord(words, 9, int.Parse, 0);
            data.SatellitesInSolution = ParseWord(words, 10, int.Parse, 0);

            return data;
        }
        private static T ParseWord<T>(string[] words, int index, Func<string, T> converter, T defaultValue)
        {
            if (words.Length > index && words[index].Length != 0)
            {
                return converter(words[index]);
            }
            return defaultValue;
        }
    }
}
