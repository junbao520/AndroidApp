using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Foundation.Spatial
{
    /// <summary>
    /// Geo辅助类
    /// 参考：
    ///     http://www.cnblogs.com/hellofox2000/archive/2010/07/13/1776159.html#2042746
    ///     http://www.oschina.net/code/snippet_200878_6939 
    ///     http://www.movable-type.co.uk/scripts/latlong.html
    ///     http://www.yourhomenow.com/house/haversine.html
    /// </summary>
    public static class GeoHelper
    {
        public const int WGS84SRID = 4326;

        /// <summary>
        /// convert degrees to radians
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private static double ToRad(this double v)
        {
            return v * System.Math.PI / 180;
        }

        /// <summary>
        /// convert radians to degrees (signed)
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private static double ToDeg(this double v)
        {
            return v * 180 / System.Math.PI;
        }

        /// <summary>
        /// convert radians to degrees (as bearing: 0...360)
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private static double ToBrng(this double v)
        {
            return (v.ToDeg() + 360) % 360;
        }

        /// <summary>
        /// 根据两点的经纬度计算两点距离
        /// 可参考:http://www.yourhomenow.com/house/haversine.html
        /// </summary>
        /// <param name="lon1">A点经度</param>
        /// <param name="lat1">A点纬度</param>
        /// <param name="lon2">B点经度</param>
        /// <param name="lat2">B点纬度</param>
        /// <returns></returns>
        public static double GetDistance2(double lon1, double lat1, double lon2, double lat2)
        {
            var dLatRad = (lat2 - lat1).ToRad();
            var dLonRad = (lon2 - lon1).ToRad();
            var lat1Rad = lat1.ToRad();
            var lat2Rad = lat2.ToRad();

            var a = System.Math.Pow(System.Math.Sin(dLatRad / 2), 2) + System.Math.Cos(lat1Rad) * System.Math.Cos(lat2Rad) * System.Math.Pow(System.Math.Sin(dLonRad / 2), 2);
            var c = 2 * System.Math.Atan2(System.Math.Sqrt(a), System.Math.Sqrt(1 - a));
            var d = LatLon.EARTH_RADIUS * c;
            return d;
        }

        /// <summary>
        /// 获取2点的航向角
        /// http://www.yourhomenow.com/house/haversine.html
        /// </summary>
        /// <param name="lon1">A点经度</param>
        /// <param name="lat1">A点纬度</param>
        /// <param name="lon2">B点经度</param>
        /// <param name="lat2">B点纬度</param>
        /// <returns></returns>
        public static double GetBearing(double lon1, double lat1, double lon2, double lat2)
        {
            var lat1Rad = lat1.ToRad();
            var lat2Rad = lat2.ToRad();
            var dLonRad = (lon2 - lon1).ToRad();



            var y = System.Math.Sin(dLonRad) * System.Math.Cos(lat2Rad);
            var x = System.Math.Cos(lat1Rad) * System.Math.Sin(lat2Rad) - System.Math.Sin(lat1Rad) * System.Math.Cos(lat2Rad) * System.Math.Cos(dLonRad);

            var a = System.Math.Atan2(y, x);
            return a.ToBrng();
        }

        /// <summary>
        /// 根据两点的经纬度计算两点距离
        /// 可参考:通过经纬度计算距离的公式 http://www.storyday.com/html/y2009/2212_according-to-latitude-and-longitude-distance-calculation-formula.html
        /// </summary>
        /// <param name="src">A点纬度</param>        
        /// <param name="dest">B点经度</param>
        /// <returns></returns>
        //public static double GetDistance(LatLon src, LatLon dest)
        //{
        //    //在纬度不变的情况下算法有问题，现换成微软的算法；
        //    if (System.Math.Abs(src.Lat) > 90 || System.Math.Abs(dest.Lat) > 90 || System.Math.Abs(src.Lon) > 180 || System.Math.Abs(dest.Lon) > 180)
        //        throw new ArgumentException("经纬度信息不正确！");

        //    //double latDis = src.RadLat - dest.RadLat;
        //    //double lonDis = src.RadLon - dest.RadLon;

        //    //double s = 2 * System.Math.Asin(System.Math.Sqrt(System.Math.Pow(System.Math.Sin(latDis / 2), 2) + System.Math.Cos(src.Lat) * System.Math.Cos(dest.Lat) * System.Math.Pow(System.Math.Sin(lonDis / 2), 2)));
        //    //s = s * LatLon.EARTH_RADIUS;

        //    var s = GetDistance(src.Lat, src.Lon, dest.Lat, dest.Lat);
        //}
        private const double EARTH_RADIUS = 6378.137; //地球半径
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }
        /// <summary>
        /// 根据经纬度计算距离
        /// </summary>
        /// <param name="lng1">经度</param>
        /// <param name="lat1">纬度</param>
        /// <param name="lng2">经度</param>
        /// <param name="lat2">纬度</param>
        /// <returns></returns>
        public static double GetDistance(double lon1, double lat1, double lon2, double lat2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);

            double radLon1 = rad(lon1);
            double radLon2 = rad(lon2);

            if (radLat1 < 0)
                radLat1 = Math.PI / 2 + Math.Abs(radLat1);// south  
            if (radLat1 > 0)
                radLat1 = Math.PI / 2 - Math.Abs(radLat1);// north  
            if (radLon1 < 0)
                radLon1 = Math.PI * 2 - Math.Abs(radLon1);// west  
            if (radLat2 < 0)
                radLat2 = Math.PI / 2 + Math.Abs(radLat2);// south  
            if (radLat2 > 0)
                radLat2 = Math.PI / 2 - Math.Abs(radLat2);// north  
            if (radLon2 < 0)
                radLon2 = Math.PI * 2 - Math.Abs(radLon2);// west  
            double x1 = EARTH_RADIUS * Math.Cos(radLon1) * Math.Sin(radLat1);
            double y1 = EARTH_RADIUS * Math.Sin(radLon1) * Math.Sin(radLat1);
            double z1 = EARTH_RADIUS * Math.Cos(radLat1);

            double x2 = EARTH_RADIUS * Math.Cos(radLon2) * Math.Sin(radLat2);
            double y2 = EARTH_RADIUS * Math.Sin(radLon2) * Math.Sin(radLat2);
            double z2 = EARTH_RADIUS * Math.Cos(radLat2);

            double d = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2) + (z1 - z2) * (z1 - z2));
            //余弦定理求夹角  
            double theta = Math.Acos((EARTH_RADIUS * EARTH_RADIUS + EARTH_RADIUS * EARTH_RADIUS - d * d) / (2 * EARTH_RADIUS * EARTH_RADIUS));
            double dist = theta * EARTH_RADIUS*1000;
            return dist;
        }


        /// <summary>
        /// 根据两点的经纬度计算两点距离
        /// 可参考:通过经纬度计算距离的公式 http://www.storyday.com/html/y2009/2212_according-to-latitude-and-longitude-distance-calculation-formula.html
        /// </summary>
        /// <param name="lon1">A点经度</param>
        /// <param name="lat1">A点纬度</param>
        /// <param name="lon2">B点经度</param>
        /// <param name="lat2">B点纬度</param>
        /// <returns></returns>
        //public static double GetDistance(double lon1, double lat1, double lon2, double lat2)
        //{
        //    LatLon src = new LatLon(lat1, lon1);
        //    LatLon dest = new LatLon(lat2, lon2);
        //    return GetDistance(src, dest);
        //}

        /// <summary>
        /// 已知点A经纬度，根据B点据A点的距离，和方位，求B点的经纬度
        /// </summary>
        /// <param name="a">已知点A</param>
        /// <param name="distance">B点到A点的距离 </param>
        /// <param name="angle">B点相对于A点的方位，12点钟方向为零度，角度顺时针增加</param>
        /// <returns>B点的经纬度坐标</returns>
        public static LatLon GetLatLon(LatLon a, double distance, double angle)
        {
            double dx = distance * 1000 * System.Math.Sin(angle * System.Math.PI / 180);
            double dy = distance * 1000 * System.Math.Cos(angle * System.Math.PI / 180);

            double lon = (dx / a.Ed + a.RadLon) * 180 / System.Math.PI;
            double lat = (dy / a.Ec + a.RadLat) * 180 / System.Math.PI;

            LatLon b = new LatLon(lat, lon);
            return b;
        }

        /// <summary>
        /// 已知点A经纬度，根据B点据A点的距离，和方位，求B点的经纬度
        /// </summary>
        /// <param name="longitude">已知点A经度</param>
        /// <param name="latitude">已知点A纬度</param>
        /// <param name="distance">B点到A点的距离</param>
        /// <param name="angle">B点相对于A点的方位，12点钟方向为零度，角度顺时针增加</param>
        /// <returns>B点的经纬度坐标</returns>
        public static LatLon GetLatLon(double longitude, double latitude, double distance, double angle)
        {
            LatLon a = new LatLon(latitude, longitude);
            return GetLatLon(a, distance, angle);
        }

        /// <summary>
        /// 获取2个角度之间的差值，控制在180度以内
        /// </summary>
        /// <param name="beginAngle"></param>
        /// <param name="endAngle"></param>
        /// <returns></returns>
        public static double GetDiffAngle(double beginAngle, double endAngle)
        {
            var diff = ((endAngle - beginAngle) + 360) % 360;
            if (diff > 180)
                diff = 360 - diff;
            return diff;
        }

        public static bool IsBetweenDiffAngle(double beginAngle, double endAngle, double diffAngle)
        {
            var diffAngle2 = GetDiffAngle(beginAngle, endAngle);
            return diffAngle2 < diffAngle;
        }

        /// <summary>
        /// 快速获取两点距离，极度不精确
        /// 全球各地纬度1°的间隔长度都相等（因为所有经线的长度都相等），大约是111km/1°。
        /// 赤道上经度1°对应在地面上的弧长大约也是111km。由于各纬线从赤道向两极递减，60°纬线上的长度为赤道上的一半，
        /// 所以在各纬线上经度差1°的弧长就不相等。在同一条纬线上（假设此纬线的纬度为α）经度1°对应的实际弧长大约为111cosαkm。
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <returns>单位：米</returns>
        public static double GetDistanceFast(double lon1, double lat1, double lon2, double lat2)
        {
            var lat = System.Math.Abs(lat1 - lat2);
            var lon = System.Math.Abs(lon1 - lon2);
            var distance = (lat + lon) * (0.75 * 111000);
            return distance;
        }

        public static bool IsLtDistanceFast(double lon1, double lat1, double lon2, double lat2, double distance)
        {
            var distance2 = GetDistanceFast(lon1, lat1, lon2, lat2);
            return distance2 < distance;
        }

        public static bool IsLtDistanceFast(Coordinate point, double lon1, double lat1, double distance)
        {
            return IsLtDistanceFast(point.Longitude, point.Latitude, lon1, lat1, distance);
        }
    }
}
