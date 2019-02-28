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
using TwoPole.Chameleon3.Foundation;
using TwoPole.Chameleon3.Foundation.Spatial;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure.Instance;
using TwoPole.Chameleon3.Infrastructure.Messages;

namespace TwoPole.Chameleon3.Infrastructure.Devices.CarSignal
{
    public class MileageUpdater : DisposableBase
    {

        private int _count = 0;
        private double _historyDistance = 0;
        private double _lastDistance = 0;
        private Coordinate _lastPoint;

        protected double ParkingInKmh { get; private set; }
        protected IMessenger Messenger { get; private set; }
        public GlobalSettings Settings { get; private set; }
        //上个信号来的时间记录
        private DateTime? _lastSignalTime { get; set; }

        public MileageUpdater(IMessenger messenger, double parkingInKmh = 1)
        {
         
            Messenger = messenger;
            ParkingInKmh = parkingInKmh;
            Settings = Singleton.GetDataService.GetSettings();
        }

        public void Execute(CarSignalInfo signalInfo)
        {


            ///当OBD速度有效时通过OBD进行里程计算
            if (Settings.MileageSource == MileageSource.OBD)
            {
                //记录上个信号时间
                if (!_lastSignalTime.HasValue)
                    _lastSignalTime = signalInfo.RecordTime;

                double timespan = (signalInfo.RecordTime - _lastSignalTime.Value).TotalMilliseconds / 1000;
                timespan = Convert.ToDouble(timespan.ToString("N3"));
                _lastSignalTime = signalInfo.RecordTime;
                if (signalInfo.Sensor.SpeedInKmh>0)
                {
                    Distance = Distance + (signalInfo.Sensor.SpeedInKmh / 3.6) * timespan;
                    //Distance = Distance + (signalInfo.Sensor.ObdSpeedInKmh*0.9/3.6)*0.1;
                }
                return;
            }

            if (!signalInfo.IsGpsValid)
                return;
            var gpsInfo = signalInfo.Gps;
            if (gpsInfo.LongitudeDegrees < 0.1 || gpsInfo.LatitudeDegrees < 0.1)
                return;
            //当在考试中时进行记录
            var currentPoint = gpsInfo.ToPoint();

            if (_count > 0 && (_lastPoint.IsEmpty() || signalInfo.CarState == CarState.Stop))
                return;

            _count++;
            if (_count >= 3)
            {
                _historyDistance += _lastPoint.Distance(currentPoint);
                _lastDistance = 0;
                _lastPoint = currentPoint;
                _count = 0;
            }
            else
            {
                if (_lastPoint.IsEmpty())
                {
                    _lastPoint = currentPoint;
                    _lastDistance = 0;
                }
                else
                {
                    _lastDistance = _lastPoint.Distance(currentPoint);
                }
            }
            Distance = _historyDistance + _lastDistance;
        }

        private double _distance;
        /// <summary>
        /// 当前行驶距离
        /// </summary>
        public double Distance
        {
            get
            {
                return _distance;
            }
            set
            {
                if (Math.Abs(value - _distance) > 0.1)
                {
                    _distance = value;
                    Messenger.Send(new TraveledDistanceChangedMessage(this, value));
                }
            }
        }

        public void Reset()
        {
            _lastPoint = Coordinate.Empty;
            _count = 0;
            _historyDistance = 0;
            _lastDistance = 0;
            Distance = 0;
        }

        protected override void Free(bool disposing)
        {
            if (disposing)
                Messenger.Unregister(this);
        }
    }
}