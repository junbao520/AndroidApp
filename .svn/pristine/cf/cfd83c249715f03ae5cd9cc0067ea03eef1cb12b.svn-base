using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure
{
    public static class AdvancedCarSignalExtensions
    {
        /// <summary>
        /// 检查远光灯
        /// </summary>
        /// <param name="advancedCarSignal"></param>
        /// <param name="sencods"></param>
        /// <param name="highBeamCount"></param>
        /// <returns></returns>
        public static bool CheckHighBeam(this IAdvancedCarSignal advancedCarSignal, double sencods, int highBeamCount = 1)
        {
            var historyTime = DateTime.Now.AddSeconds(-sencods);
            return advancedCarSignal.CheckHighBeam(historyTime, highBeamCount);
        }

        /// <summary>
        /// 检查远光灯
        /// </summary>
        /// <param name="advancedCarSignal"></param>
        /// <param name="historyTime"></param>
        /// <param name="sencods"></param>
        /// <param name="highBeamCount"></param>
        /// <returns></returns>
        public static bool CheckHighBeam(this IAdvancedCarSignal advancedCarSignal, DateTime historyTime, double sencods, int highBeamCount = 1)
        {
            var cachedTime = DateTime.Now.AddSeconds(-sencods);
            var time = cachedTime < historyTime ? historyTime : cachedTime;
            return advancedCarSignal.CheckHighBeam(time, highBeamCount);
        }
    }
}
