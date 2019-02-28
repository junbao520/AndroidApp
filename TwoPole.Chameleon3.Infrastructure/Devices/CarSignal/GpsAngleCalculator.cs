using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TwoPole.Chameleon3.Infrastructure.Instance;
using TwoPole.Chameleon3.Foundation.Spatial;

namespace TwoPole.Chameleon3.Infrastructure.Devices.CarSignal
{
    public class GpsAngleCalculator
    {
        public ICarSignalSet CarSignalSet { get; private set; }
        //protected ICarSignalSet CarSignalSet
        //{
        //    get { return Singleton.GetCarSignalSet; }
        //}

        public GpsAngleCalculator(ICarSignalSet signalSet)
        {
             CarSignalSet = signalSet;
        }

        public double CalculateAngle(GpsInfo current)
        {
            //return current.AngleDegrees;
            foreach (var gps in CarSignalSet.Where(x => x.IsGpsValid).Select(x => x.Gps))
            {
                var distance = GeoHelper.GetDistanceFast(current.LongitudeDegrees, current.LatitudeDegrees,
                    gps.LongitudeDegrees, gps.LatitudeDegrees);
                if (distance > 0.5)
                {
                    var angle = GeoHelper.GetBearing(gps.LongitudeDegrees, gps.LatitudeDegrees, current.LongitudeDegrees,
                        current.LatitudeDegrees);
                    return angle;
                }
            }
            return double.NaN;
        }
    }
}