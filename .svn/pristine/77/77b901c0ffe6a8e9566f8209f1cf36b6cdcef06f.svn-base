using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPole.Chameleon3.Foundation.Spatial;

namespace TwoPole.Chameleon3.Infrastructure.Map
{
    public class MapSet : IMapSet
    {
        public readonly static IMapSet Empty = new MapSet(new List<MapPoint>(0));

        public MapPoint[] MapPoints { get; set; }

        public MapSet(IEnumerable<MapPoint> points)
        {
            if (points == null)
                throw new ArgumentNullException("points");

            MapPoints = points as MapPoint[] ?? points.ToArray();
        }

        public IEnumerator<MapPoint> GetEnumerator()
        {
            return MapPoints.Select(x => x).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public MapPoint this[int index]
        {
            get
            {
                if (index < 0 || index >= MapPoints.Length)
                    throw new ArgumentOutOfRangeException("index");

                return MapPoints[index];
            }
        }

        public int Count { get { return this.MapPoints.Length; } }
    }
}
