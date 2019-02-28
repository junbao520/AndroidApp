using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Foundation.Spatial;

namespace TwoPole.Chameleon3.Infrastructure
{
    public static class DomainExtensions
    {
        public static Coordinate ToPoint(this MapLinePoint linePoint)
        {
            return new Coordinate(linePoint.Longitude, linePoint.Latitude, linePoint.Altitude.GetValueOrDefault(), linePoint.Angle);
        }

        #region MapPoint
        private static MapPoint ToMapPoint(this MapLinePoint linePoint, int index)
        {
            var mapPoint = new MapPoint(linePoint.ToPoint(),
                index, linePoint.Name, linePoint.PointType, linePoint.SpeedLimit);
            ParseProperties(mapPoint, linePoint.Settings);

            return mapPoint;
        }

        private static void ParseProperties(MapPoint point, IEnumerable<Setting> settings)
        {
            if (settings != null)
            {
                foreach (var setting in settings)
                {
                    if (!string.IsNullOrEmpty(setting.Value))
                        point.Properties[setting.Key] = setting.Value;
                }
            }
        }

        public static IEnumerable<MapPoint> ToMapPoints(this IEnumerable<MapLinePoint> linePoints)
        {
            var index = 0;
            foreach (var mapLinePoint in linePoints)
            {
                yield return mapLinePoint.ToMapPoint(index);
                index++;
            }
        }
        #endregion
    }
}
