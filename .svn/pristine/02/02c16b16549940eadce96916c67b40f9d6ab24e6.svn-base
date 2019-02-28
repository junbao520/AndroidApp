using System;
using System.Text;
using TwoPole.Chameleon3.Foundation.Gps;

namespace TwoPole.Chameleon3.Infrastructure
{
    public sealed class GpsInfo
    {
        /// <summary>
        /// Gps时间
        /// </summary>
        public DateTime UtcTime { get; set; }
        /// <summary>
        /// GPS转换后的本地时间
        /// </summary>
        public DateTime LocalTime { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double LatitudeDegrees { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double LongitudeDegrees { get; set; }
        /// <summary>
        /// 正北方向的距离
        /// </summary>
        public double NorthingMeters { get; set; }
        /// <summary>
        /// 正东方向的距离
        /// </summary>
        public double EastingMeters { get; set; }
        /// <summary>
        /// 与正北方向夹角
        /// </summary>
        public double AngleDegrees { get; set; }
        /// <summary>
        /// 速度
        /// </summary>
        public double SpeedInKmh { get; set; }
        /// <summary>
        /// 俯仰角
        /// </summary>
        public double ElevationDegrees { get; set; }
        /// <summary>
        /// 海拔高度
        /// </summary>
        public double AltitudeMeters { get; set; }
        /// <summary>
        /// Gps精度
        /// </summary>
        public Quality FixQuality { get; set; }
        /// <summary>
        /// The Fixed Satellite Count
        /// </summary>
        public int FixedSatelliteCount { get; set; }
        /// <summary>
        /// The Tracked Satellite Count
        /// </summary>
        public int TrackedSatelliteCount { get; set; }

        public DateTime RecordTime { get; set; }
        /// <summary>
        /// 是否高精度版本
        /// </summary>
        public  bool HighPrecisionVersion { get; set; }


        public GpsInfo()
        {
            RecordTime = DateTime.Now;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("UtcTime: ").Append(UtcTime)
                .Append("LocalTime:").Append(LocalTime)
                .Append(" Longitude: ").Append(LongitudeDegrees)
                .Append(" Latitude: ").Append(LatitudeDegrees)
                .Append(" Altitude: ").Append(AltitudeMeters)
                .Append(" Speed: ").Append(SpeedInKmh)
                .Append(" Easting: ").Append(EastingMeters)
                .Append(" Northing: ").Append(NorthingMeters)
                .Append(" Angle: ").Append(AngleDegrees)
                .Append(" Quality: ").Append(FixQuality)
                .Append(" Satellite Count: ").Append(FixedSatelliteCount);
            return sb.ToString();
        }
    }
}
