using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Domain;

namespace TwoPole.Chameleon3.Foundation.Spatial
{
    public class MapPointTypeInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 点位图标
        /// </summary>
        public string IconUrl { get; set; }
        /// <summary>
        /// 点位类型
        /// </summary>
        public MapPointType PointType { get; set; }
        /// <summary>
        /// 限速
        /// </summary>
        public double? SpeedLimit { get; set; }
        /// <summary>
        /// 其它参数
        /// </summary>
        public string Parameters { get; set; }
    }
}
