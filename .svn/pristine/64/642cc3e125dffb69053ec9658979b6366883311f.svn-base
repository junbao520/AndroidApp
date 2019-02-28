using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Infrastructure;
using TwoPole.Chameleon3.Foundation.Spatial;

namespace TwoPole.Chameleon3.Infrastructure.Map
{
    /// <summary>
    /// 搜索车模（基点）所在的地图位置
    /// </summary>
    public interface IGpsPointSearcher
    {
        /// <summary>
        /// 设置地图点
        /// </summary>
        /// <param name="points"></param>
        void SetMapPoints(IEnumerable<MapPoint> points);

        /// <summary>
        /// 搜索当前点位
        /// </summary>
        /// <param name="signalInfo">车载信号</param>
        /// <returns></returns>
        MapPoint[] Search(CarSignalInfo signalInfo);
    }
}
