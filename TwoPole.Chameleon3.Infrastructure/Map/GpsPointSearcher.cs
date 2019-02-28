using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Domain;
using TwoPole.Chameleon3.Foundation.Spatial;
using TwoPole.Chameleon3.Infrastructure.Instance;

namespace TwoPole.Chameleon3.Infrastructure.Map
{
    public class GpsPointSearcher : IGpsPointSearcher
    {
      
        public MapPoint[] MapPoints { get; private set; }
        protected ILog Logger { get; set; }

        public GpsPointSearcher()
        {
            MapPoints = new MapPoint[0];
            Logger = Singleton.GetLogManager;
        }

        public void SetMapPoints(IEnumerable<MapPoint> points)
        {
            MapPoints = points == null ? new MapPoint[0] : points.ToArray();
        }

        /// <summary>
        /// 查找同方向的最近点位
        /// </summary>
        /// <param name="signalInfo"></param>
        /// <returns></returns>
        public MapPoint[] Search(CarSignalInfo signalInfo)
        {
            //无航向角（停车时）不搜索点位
            if (MapPoints.Length == 0 || !signalInfo.Gps.AngleDegrees.IsValidAngle() || signalInfo.CarState == CarState.Stop)
                return new MapPoint[0];

            var gps = signalInfo.Gps;
            //
            //搜索20米内同方向（相同15角度类）的所有点位
            var query = from a in MapPoints
                        where
                             a.PointType != MapPointType.Normal &&
                             GeoHelper.IsBetweenDiffAngle(a.Point.Angle, gps.AngleDegrees,15) &&
                            (GeoHelper.GetDistance(a.Point.Longitude, a.Point.Latitude, gps.LongitudeDegrees, gps.LatitudeDegrees) <=15)
                        select a;

            var items = query.ToArray();

            if (items.Any())
            {
                foreach (var _item in items)
                {
                    Logger.InfoFormat("搜索符合点：{0}-{1}-{2}-{3}", _item.Name, _item.Point.Longitude, _item.Point.Latitude, _item.Point.Angle);
                    Logger.InfoFormat("当前GPS：{0}-{1}-{2}", signalInfo.Gps.LongitudeDegrees, signalInfo.Gps.LatitudeDegrees, signalInfo.Gps.AngleDegrees);
                }
            }
            return items;
            //return null;
        }
    }
}
