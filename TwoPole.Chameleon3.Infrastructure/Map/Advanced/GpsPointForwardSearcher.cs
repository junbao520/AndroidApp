//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Chameleon.Foundation;
//using Common.Logging;
//using Microsoft.SqlServer.Types;

//namespace TwoPole.Chameleon3.Infrastructure.Spatial.Advanced
//{
//    /// <summary>
//    /// 只向前的地图搜索器，适应于平原地区
//    /// 不支持中途任意掉头
//    /// 不支持循环考试
//    /// </summary>
//    public class GpsPointForwardSearcher : IGpsPointSearcher
//    {
//        protected ILog Logger { get; private set; }

//        public MapPointNode CurrentNode { get; private set; }

//        public GpsPointForwardSearcher()
//        {
//            Logger = LogManager.GetLogger<GpsPointForwardSearcher>();
//        }

//        public MapPointNode Search(GpsInfo gpsInfo, MapPointNode[] points)
//        {
//            if (points == null || points.Length <= 0)
//            {
//                Logger.Debug("无Gps地图信息点");
//                return CurrentNode;
//            }
//            //无航向角（停车时）不搜索点位，
//            if (double.IsNaN(gpsInfo.AngleDegrees) || Math.Abs(gpsInfo.AngleDegrees) <= 0.01)
//            {
//                Logger.Debug("GPS信号无航向角");
//                return CurrentNode;
//            }
//            var currentSqlPoint = gpsInfo.ToPoint().ToSqlPoint();
//            if (CurrentNode == null)
//            {
//                CurrentNode = FindNearestPoint(currentSqlPoint, points);
//                return CurrentNode;
//            }

//            //double currentDistance = CurrentNode.Next == null ?
//            //    currentSqlPoint.STDistance(CurrentNode.MapPoint.SqlPoint).Value :
//            //    currentSqlPoint.STDistance(CurrentNode.SqlLine).Value;
//            ////向前
//            //if (CurrentNode.Next != null)
//            //{
//            //    var nextDistance = CurrentNode.Next.SqlLine == null
//            //        ? currentSqlPoint.STDistance(CurrentNode.Next.MapPoint.SqlPoint)
//            //        : currentSqlPoint.STDistance(CurrentNode.Next.SqlLine).Value;
//            //    if (nextDistance <= currentDistance)
//            //    {
//            //        _backCount = 0;
//            //        CurrentNode = CurrentNode.Next;
//            //        Logger.DebugFormat("重新搜索地图点{0}:{1}", CurrentNode.MapPoint.Index, CurrentNode.MapPoint.Name);
//            //        return CurrentNode;
//            //    }
//            //}
//            //else
//            //{
//            //    //到达最后一个点后，重新动态搜索
//            //    _backCount = 0;
//            //    CurrentNode = FindNearestPointWithSameDirection(currentSqlPoint, gpsInfo.AngleDegrees, points);
//            //    Logger.DebugFormat("到达地图终点，重新搜索地图点{0}:{1}", CurrentNode.MapPoint.Index, CurrentNode.MapPoint.Name);
//            //    return CurrentNode;
//            //}

//            ////todo: 不能向后搜索
//            ////检测倒退情况
//            //if (!CurrentNode.IsStartNode)
//            //{
//            //    var previousNode = points[CurrentNode.MapPoint.Index - 1];
//            //    var previousDistance = currentSqlPoint.STDistance(previousNode.SqlLine).Value;
//            //    if (previousDistance < currentDistance)
//            //    {
//            //        _backCount++;
//            //        Logger.DebugFormat("在地图点{0}:{1}检测到后溜:{2}/{3}",
//            //            CurrentNode.MapPoint.Index, CurrentNode.MapPoint.Name,
//            //            _backCount, DefaultBackMaxPoints);

//            //        //如果后退达到规定的点数，重新搜索点位；
//            //        if (_backCount >= DefaultBackMaxPoints)
//            //        {
//            //            CurrentNode = FindNearestPointWithSameDirection(currentSqlPoint, gpsInfo.AngleDegrees, points);
//            //            Logger.DebugFormat("检测到连续后溜{0}个点位，地图自动重新搜索:{1}：{2}",
//            //                _backCount, CurrentNode.MapPoint.Index, CurrentNode.MapPoint.Name);
//            //            _backCount = 0;
//            //            return CurrentNode;
//            //        }
//            //    }
//            //}

//            return CurrentNode;
//        }

//        /// <summary>
//        /// 重新进行定位搜索
//        /// </summary>
//        public void Reset()
//        {
//            CurrentNode = null;
//        }

//        /// <summary>
//        /// 查找同方向的最近点位
//        /// </summary>
//        /// <param name="currentPoint"></param>
//        /// <param name="nodes"></param>
//        /// <returns></returns>
//        private MapPointNode FindNearestPoint(SqlGeography currentPoint,
//            IEnumerable<MapPointNode> nodes)
//        {
//            var query = from a in nodes
//                        where a.SqlLine != null
//                        let distance = currentPoint.STDistance(a.SqlLine)
//                        select new
//                        {
//                            mapPoint = a,
//                            distance
//                        };

//            var items = query.ToArray();
//            if (items.Length == 0)
//                return null;

//            var minMapPoint = items.MinEx(x => x.distance, f => f.mapPoint);
//            return minMapPoint;
//        }
//    }
//}
