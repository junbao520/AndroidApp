using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Foundation.Spatial;

namespace TwoPole.Chameleon3.Infrastructure.Map
{
    /// <summary>
    /// 地图集合（项目点位）
    /// </summary>
    public interface IMapSet : IEnumerable<MapPoint>
    {
        /// <summary>
        /// 项目点位
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        MapPoint this[int index] { get; }

        /// <summary>
        /// 项目数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 所有项目点位
        /// </summary>
        MapPoint[] MapPoints { get; }
    }
}
