using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Domain;

namespace TwoPole.Chameleon3.Foundation.Spatial
{
    public class MapPoint
    {
        public Coordinate Point { get; private set; }

        public int Index { get; private set; }

        public string Name { get; private set; }

        public MapPointType PointType { get; private set; }

        public double? SpeedLimit { get; private set; }

        public IDictionary<string, object> Properties { get; private set; }

        public MapPoint(Coordinate point, int index, string name, MapPointType type, double? speedLimit = null)
        {
            this.Point = point;
            this.Index = index;
            this.Name = name;
            this.PointType = type;
            this.SpeedLimit = speedLimit;
            this.Properties = new Dictionary<string, object>();
        }

        public double Distance(MapPoint mapPoint)
        {
            return this.Point.Distance(mapPoint.Point);
        }

        public double Distance(Coordinate point)
        {
            return this.Point.Distance(point);
        }

        public object GetPropertyValue(string key)
        {
            object v;
            this.Properties.TryGetValue(key, out v);
            return v;
        }
        
        public T GetPropertyValue<T>(string key)
        {
            var v = GetPropertyValue(key);
            if (v != null)
                return (T) v;
            return default (T);
        }

        public string ToKey()
        {
            return string.Format("{0}-{1}", Index, (int)PointType);
        }
    }
}
