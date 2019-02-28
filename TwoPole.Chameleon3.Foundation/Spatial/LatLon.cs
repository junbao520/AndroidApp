using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Foundation.Spatial
{
    /// <summary>
    /// 经纬度表示类
    /// 经纬度计算主要有两种：
    /// 1. 知道两点的经纬度值，计算两点间的距离
    /// 2. 知道一点的经纬度，知道另一点相对于此点的角度，距离。计算另一点的经纬度信息
    /// http://blog.csdn.net/fdnike/archive/2007/07/18/1696603.aspx
    /// </summary>
    public class LatLon
    {
        /// <summary>
        /// 赤道半径 earth radius
        /// </summary>
        public const double EARTH_RADIUS = 6378137;

        /// <summary>
        /// 极半径 polar radius
        /// </summary>
        public const double POLAR_RADIUS = 6356725;

        /// <summary>
        /// 
        /// </summary>
        public LatLon()
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lat">维度</param>
        /// <param name="lon">经度</param>
        public LatLon(double lat, double lon)
        {
            this.Lat = lat;
            this.Lon = lon;
        }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double Lon { get; set; }

        /// <summary>
        /// 纬度的弧度
        /// </summary>
        public double RadLat { get { return Lat * System.Math.PI / 180; } }

        /// <summary>
        /// 经度的弧度
        /// </summary>
        public double RadLon { get { return Lon * System.Math.PI / 180; } }

        /// <summary>
        /// ?
        /// </summary>
        public double Ec { get { return POLAR_RADIUS + (EARTH_RADIUS - POLAR_RADIUS) * (90 - Lat) / 90; } }

        /// <summary>
        /// ?
        /// </summary>
        public double Ed { get { return Ec * System.Math.Cos(RadLat); } }
    }
}