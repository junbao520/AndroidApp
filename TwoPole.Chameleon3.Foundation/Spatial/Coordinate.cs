using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Foundation.Spatial
{
    /// <summary>
    /// 
    /// </summary>
    public struct Coordinate
    {
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude;
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude;
        /// <summary>
        /// 海拔
        /// </summary>
        public double Altitude;
        /// <summary>
        /// 航向角
        /// </summary>
        public double Angle;

        public readonly static Coordinate Empty = new Coordinate();

        public Coordinate(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
            Altitude = double.NaN;
            Angle = double.NaN;
        }

        public Coordinate(double longitude, double latitude, double altitude)
        {
            Longitude = longitude;
            Latitude = latitude;
            Altitude = altitude;
            Angle = double.NaN;
        }

        public Coordinate(double longitude, double latitude, double altitude, double angle)
        {
            Longitude = longitude;
            Latitude = latitude;
            Altitude = altitude;
            Angle = angle;
        }

        public bool IsEmpty()
        {
            return Longitude == 0 && Latitude == 0;
        }

        public double Distance(Coordinate target)
        {
            return GeoHelper.GetDistance(this.Longitude, this.Latitude, target.Longitude, target.Latitude);
           // return 0;
        }
    }
}
