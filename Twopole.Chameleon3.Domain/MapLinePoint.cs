using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Domain
{
     /// <summary>
     /// 地图点位
     /// </summary>
     public class MapLinePoint 
     {
         public MapLinePoint()
         {
             this.Settings = new HashSet<Setting>();
         }

         /// <summary>
         /// 点位名称
         /// </summary>
         public string Name { get; set; }

         /// <summary>
         /// 经度
         /// </summary>
         public double Longitude { get; set; }

         /// <summary>
         /// 纬度
         /// </summary>
         public double Latitude { get; set; }

         /// <summary>
         /// 海拔
         /// </summary>
         public  double? Altitude { get; set; }

         /// <summary>
         /// 航向角
         /// </summary>
         public  double Angle { get; set; }

         /// <summary>
         /// 排序号
         /// </summary>
         public  int SequenceNumber { get; set; }

         /// <summary>
         /// 地图点位类型
         /// </summary>
         public MapPointType PointType { get; set; }

         /// <summary>
         /// 地图速度限制
         /// </summary>
         public  double? SpeedLimit { get; set; }

         /// <summary>
         /// 其它配置信息
         /// </summary>
         public ISet<Setting> Settings { get; set; }
     }
}
