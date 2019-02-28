using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Foundation.Spatial;

namespace TwoPole.Chameleon3.Infrastructure
{
    public static class SpatialExtensions
    {
        public static double CalculateDistances(this IEnumerable<MapLinePoint> points)
        {
            if (points == null)
                return 0;

            var items = points.ToArray();
            if (items.Length == 0)
                return 0;

            var last = items[0];
            var sum = 0d;
            foreach (var point in items)
            {
               var len = GeoHelper.GetDistance(last.Longitude, last.Latitude, point.Longitude, point.Latitude);
                if (!double.IsNaN(len))
                    sum += len;
                last = point;
            }
            return sum;
        }

        public static double CalculateDistances(this IEnumerable<MapLinePoint> points, double longitude, double latitude)
        {
            var pointsDistance = CalculateDistances(points);
            if (points.Any())
            {
                var lastPoint = points.Last();
                var len = GeoHelper.GetDistance(lastPoint.Longitude, lastPoint.Latitude, longitude, latitude);
                if (!double.IsNaN(len))
                    pointsDistance += len;
            }
            return pointsDistance;
        }

        public static double CalculateDistances(this IEnumerable<Coordinate> points)
        {
            if (points == null)
                return 0;

            var items = points.ToArray();
            if (items.Length == 0)
                return 0;

            var last = items[0];
            var sum = 0d;
            foreach (var point in items)
            {
         
                var len = GeoHelper.GetDistance(last.Longitude, last.Latitude, point.Longitude, point.Latitude);
                if (!double.IsNaN(len))
                    sum += len;
                last = point;
            }
            return sum;
        }

        public static Coordinate ToPoint(this GpsInfo gpsInfo)
        {
            return new Coordinate(gpsInfo.LongitudeDegrees, gpsInfo.LatitudeDegrees, gpsInfo.AltitudeMeters, gpsInfo.AngleDegrees);
        }

        public static bool IsValidAngle(this double angle)
        { 
            return !double.IsNaN(angle) && Math.Abs(angle) > 0.01;
        }

        public static bool IsValidAngle(this GpsInfo gps)
        {
            return gps.AngleDegrees.IsValidAngle();
        }

        public static IEnumerable<GpsInfo> ExceptInvalidAngle(this IEnumerable<GpsInfo> source)
        {
            return source.Where(IsValidAngle);
        }
    }
}
