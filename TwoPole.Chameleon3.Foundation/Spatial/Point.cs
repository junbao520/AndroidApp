using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Foundation.Spatial
{
    public struct Point
    {
        public double Latitude;

        public double Longitude;

        public double? Altitude;

        public Point(double longitude, double latitude, double? altitude = null)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
        }
    }
}
