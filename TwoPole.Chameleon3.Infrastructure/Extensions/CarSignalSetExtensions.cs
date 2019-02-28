using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Infrastructure
{
    public static class CarSignalSetExtensions
    {
        /// <summary>
        /// 查找N秒前的缓存数据
        /// </summary>
        /// <param name="carSensorSet"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static CarSignalInfo[] QueryCachedSeconds(this ICarSignalSet carSensorSet, double seconds)
        {
            var cachedDateTime = DateTime.Now.AddSeconds(-seconds);
            return carSensorSet.Query(cachedDateTime);
        }

        /// <summary>
        /// 查找N秒前的缓存数据，缓存时间不能超过历史时刻：HistoryTime
        /// </summary>
        /// <param name="carSensorSet"></param>
        /// <param name="historyTime">历史时刻：HistoryTime</param>
        /// <param name="seconds">N秒前数据</param>
        /// <returns></returns>
        public static CarSignalInfo[] QueryCachedSeconds(this ICarSignalSet carSensorSet, DateTime historyTime, double seconds)
        {
            var cachedDateTime = DateTime.Now.AddSeconds(-seconds);
            var time = cachedDateTime < historyTime ? historyTime : cachedDateTime;
            return carSensorSet.Query(time);
        }


        ///// <summary>
        ///// N秒前是否有信号；默认取5个点
        ///// </summary>
        ///// <param name="carSensorSet">传感器集合</param>
        ///// <param name="senconds">N秒前</param>
        ///// <param name="filter">检查条件</param>
        ///// <returns></returns>
        //public static bool AnySensor(this ICarSignalSet carSensorSet, double senconds, Func<CarSignalInfo, bool> filter)
        //{
        //    var checkedTime = DateTime.Now.AddSeconds(-senconds);
        //    return carSensorSet.SkipWhile(x => x.RecordTime > checkedTime).Take(5).Any(filter);
        //}
    }
}
